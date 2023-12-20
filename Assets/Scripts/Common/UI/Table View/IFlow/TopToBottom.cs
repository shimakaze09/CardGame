using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TheLiquidFire.DataTypes;

namespace TheLiquidFire.UI
{
    public class TopToBottom : IFlow
    {
        private ScrollRect ScrollRect;
        private ISpacer Spacer;
        private int ViewHeight;

        public TopToBottom(ScrollRect scrollRect, ISpacer spacer)
        {
            ScrollRect = scrollRect;
            Spacer = spacer;
            var rt = scrollRect.transform as RectTransform;
            ViewHeight = Mathf.CeilToInt(rt.sizeDelta.y);
        }

        public void ConfigureCell(TableViewCell cell, int index)
        {
            var rt = cell.transform as RectTransform;
            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(1, 1);
            rt.pivot = new Vector2(0, 1);

            var height = Spacer.GetSize(index);
            rt.sizeDelta = new Vector2(0, height);

            cell.SetShowAnchors(TextAnchor.UpperLeft, TextAnchor.UpperLeft);
            cell.SetHideAnchors(TextAnchor.UpperLeft, TextAnchor.UpperRight);
        }

        public Point GetVisibleCellRange()
        {
            var startY = Mathf.RoundToInt(ScrollRect.content.anchoredPosition.y);
            var endY = startY + ViewHeight;
            return Spacer.GetVisibleCellRange(startY, endY);
        }

        public Vector2 GetCellOffset(int index)
        {
            return GetCellOffset(index, 0);
        }

        public Vector2 GetCellOffset(int index, int padding)
        {
            var position = Spacer.GetPosition(index) + padding;
            return new Vector2(0, -position);
        }
    }
}