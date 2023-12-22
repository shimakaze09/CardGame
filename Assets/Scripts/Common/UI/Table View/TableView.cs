using System;
using System.Collections.Generic;
using TheLiquidFire.DataTypes;
using TheLiquidFire.Extensions;
using TheLiquidFire.Pooling;
using UnityEngine;
using UnityEngine.UI;

namespace TheLiquidFire.UI
{
    [RequireComponent(typeof(ScrollRect))]
    [RequireComponent(typeof(IntKeyedPooler))]
    public class TableView : MonoBehaviour
    {
        #region Delegates

        public Func<TableView, int, int> sizeForCellInTableView;
        public Func<TableView, int> cellCountForTableView;
        public Action<TableView, TableViewCell, int> willShowCellAtIndex;
        public Action<TableView, TableViewCell> willHideCell;

        #endregion

        #region Fields

        public int cellSize = 100;
        public Point visibleRange { get; private set; }

        private ScrollRect scrollRect;
        private IntKeyedPooler cellPooler;
        private ISpacer spacer;
        private IContainer container;

        #endregion

        #region MonoBehaviour

        private void OnEnable()
        {
            scrollRect = GetComponent<ScrollRect>();
            cellPooler = GetComponent<IntKeyedPooler>();
            scrollRect.onValueChanged.AddListener(OnScroll);
            cellPooler.didDequeueForKey = OnDidDequeueForKey;
            cellPooler.willEnqueue = OnWillEnqueue;
        }

        private void OnDisable()
        {
            scrollRect.onValueChanged.RemoveListener(OnScroll);
            cellPooler.didDequeueForKey = null;
            cellPooler.willEnqueue = null;
        }

        #endregion

        #region Event Handlers

        private void OnScroll(Vector2 pos)
        {
            Refresh();
        }

        private void OnDidDequeueForKey(Poolable item, int key)
        {
            var cell = item.GetComponent<TableViewCell>();
            cell.transform.SetParent(scrollRect.content);
            cell.transform.localScale = Vector3.one;
            cell.gameObject.SetActive(true);

            container.Flow.ConfigureCell(cell, key);
            var offset = container.Flow.GetCellOffset(key);
            cell.Show(offset);

            if (willShowCellAtIndex != null)
                willShowCellAtIndex(this, cell, key);
        }

        private void OnWillEnqueue(Poolable item)
        {
            var cell = item.GetComponent<TableViewCell>();
            cell.Hide();

            if (willHideCell != null)
                willHideCell(this, cell);
        }

        #endregion

        #region Public

        public void Reload()
        {
            Setup();
            cellPooler.EnqueueAll();
            visibleRange = new Point(int.MaxValue, int.MinValue);

            if (cellCountForTableView == null)
                return;
            var rowCount = cellCountForTableView(this);

            if (sizeForCellInTableView != null)
                spacer = new NonUniformSpacer(index => { return sizeForCellInTableView(this, index); }, rowCount);
            else
                spacer = new UniformSpacer(cellSize, rowCount);

            if (scrollRect.horizontal)
                container = new HorizontalContainer(scrollRect, spacer);
            else
                container = new VerticalContainer(scrollRect, spacer);

            container.AutoSize();
            scrollRect.content.anchoredPosition = Vector2.zero;
            Refresh();
        }

        public void InsertCell(int index)
        {
            InsertCellData(index);
            ApplyNewPositions();
        }

        public void InsertCells(HashSet<int> indexSet)
        {
            var indices = indexSet.ToSortedList();
            for (var i = 0; i < indices.Count; ++i)
                InsertCellData(indices[i]);
            ApplyNewPositions();
        }

        public void RemoveCell(int index)
        {
            RemoveCellData(index);
            ApplyNewPositions();
        }

        public void RemoveCells(HashSet<int> indexSet)
        {
            var indices = indexSet.ToSortedList();
            for (var i = indices.Count - 1; i >= 0; --i)
                RemoveCellData(indices[i]);
            ApplyNewPositions();
        }

        #endregion

        #region Private

        private void Setup()
        {
            scrollRect = GetComponent<ScrollRect>();
            cellPooler = GetComponent<IntKeyedPooler>();
            scrollRect.onValueChanged.AddListener(OnScroll);
            cellPooler.didDequeueForKey = OnDidDequeueForKey;
            cellPooler.willEnqueue = OnWillEnqueue;
        }

        private void Refresh()
        {
            var range = container.Flow.GetVisibleCellRange();
            if (visibleRange == range)
                return;

            // Step 1: Reclaim any cells that are out of bounds
            for (var i = visibleRange.x; i <= visibleRange.y; ++i)
                if (i < range.x || i > range.y)
                    cellPooler.EnqueueByKey(i);

            // Step 2: Load any cells that are missing
            for (var i = range.x; i <= range.y; ++i) cellPooler.DequeueByKey(i);

            visibleRange = range;
        }

        private void InsertCellView(int index)
        {
            var cell = cellPooler.DequeueScriptByKey<TableViewCell>(index);
            var offset = container.Flow.GetCellOffset(index);
            cell.Insert(offset);
        }

        private void RemoveCellView(int index)
        {
            var cell = cellPooler.GetScript<TableViewCell>(index);
            cellPooler.Collection.Remove(index);
            var tweener = cell.Remove();
            tweener.completedEvent += (sender, e) => { cellPooler.EnqueueScript(cell); };
        }

        private void InsertCellData(int index)
        {
            var keys = SortedKeys();
            for (var i = keys.Count - 1; i >= 0; --i)
            {
                var key = keys[i];
                if (key < index)
                    break;

                var item = cellPooler.Collection[key];
                cellPooler.Collection.Remove(key);
                cellPooler.Collection.Add(key + 1, item);
                var offset = container.Flow.GetCellOffset(key);
                item.GetComponent<TableViewCell>().Pin(offset);
            }

            spacer.Insert(index);

            visibleRange = container.Flow.GetVisibleCellRange();
            if (index >= visibleRange.x && index <= visibleRange.y)
                InsertCellView(index);
        }

        private void RemoveCellData(int index)
        {
            var keys = SortedKeys();
            for (var i = 0; i < keys.Count; ++i)
            {
                var key = keys[i];
                if (key < index) continue;

                if (key == index)
                {
                    RemoveCellView(key);
                }
                else
                {
                    var item = cellPooler.Collection[key];
                    cellPooler.Collection.Remove(key);
                    cellPooler.Collection.Add(key - 1, item);
                    var offset = container.Flow.GetCellOffset(key);
                    item.GetComponent<TableViewCell>().Pin(offset);
                }
            }

            var height = spacer.GetSize(index);
            spacer.Remove(index);

            visibleRange = container.Flow.GetVisibleCellRange();
            for (var i = visibleRange.x; i <= visibleRange.y; ++i)
            {
                if (cellPooler.HasKey(i))
                    continue;
                var cell = cellPooler.DequeueScriptByKey<TableViewCell>(i);
                var offset = container.Flow.GetCellOffset(i, height);
                cell.Pin(offset);
            }
        }

        private void ApplyNewPositions()
        {
            container.AutoSize();
            foreach (var key in cellPooler.Collection.Keys)
            {
                var index = key;
                var offset = container.Flow.GetCellOffset(index);
                var cell = cellPooler.GetScript<TableViewCell>(index);
                var tweener = cell.Shift(offset);
                if (tweener != null)
                    tweener.completedEvent += (sender, e) =>
                    {
                        if (index < visibleRange.x || index > visibleRange.y)
                            cellPooler.EnqueueByKey(index);
                    };
            }
        }

        private List<int> SortedKeys()
        {
            var keys = new List<int>(cellPooler.Collection.Keys);
            keys.Sort();
            return keys;
        }

        #endregion
    }
}