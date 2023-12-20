using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TheLiquidFire.DataTypes;

namespace TheLiquidFire.UI
{
    public class BottomToTop : IFlow
    {
        private ScrollRect ScrollRect;
        private ISpacer Spacer;
        private int ViewHeight;

        public BottomToTop(ScrollRect scrollRect, ISpacer spacer)
        {
            ScrollRect = scrollRect;
            Spacer = spacer;
            var rt = scrollRect.transform as RectTransform;
            ViewHeight = Mathf.CeilToInt(rt.sizeDelta.y);
        }

        public void ConfigureCell(TableViewCell cell, int index)
        {
            var rt = cell.transform as RectTransform;
            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(1, 0);
            rt.pivot = new Vector2(0, 0);

            var height = Spacer.GetSize(index);
            rt.sizeDelta = new Vector2(0, height);

            cell.SetShowAnchors(TextAnchor.LowerLeft, TextAnchor.LowerLeft);
            cell.SetHideAnchors(TextAnchor.LowerLeft, TextAnchor.LowerRight);
        }

        public Point GetVisibleCellRange()
        {
            var startY = -Mathf.RoundToInt(ScrollRect.content.anchoredPosition.y);
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
            return new Vector2(0, position);
        }
    }
}