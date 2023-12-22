using System;
using System.Collections.Generic;
using TheLiquidFire.DataTypes;
using TheLiquidFire.Extensions;
using UnityEngine;

namespace TheLiquidFire.UI
{
    public class NonUniformSpacer : ISpacer
    {
        private readonly List<int> Positions;
        private readonly Func<int, int> SizeForIndex;
        private readonly List<int> Sizes;

        public NonUniformSpacer(Func<int, int> sizeForIndex, int cellCount)
        {
            Sizes = new List<int>(cellCount);
            Positions = new List<int>(cellCount);
            SizeForIndex = sizeForIndex;

            TotalSize = 0;
            for (var i = 0; i < cellCount; ++i)
            {
                var size = SizeForIndex(i);
                Sizes.Add(size);
                Positions.Add(TotalSize);
                TotalSize += size;
            }
        }

        public int TotalSize { get; private set; }

        public int GetSize(int index)
        {
            return Sizes[index];
        }

        public int GetPosition(int index)
        {
            return Positions[index];
        }

        public void Insert(int index)
        {
            var size = SizeForIndex(index);
            TotalSize += size;

            if (Sizes.Count == index)
            {
                if (Positions.Count == 0)
                    Positions.Add(0);
                else
                    Positions.Add(Positions.Last() + Sizes.Last());
                Sizes.Add(size);
            }
            else
            {
                Sizes.Insert(index, size);
                Positions.Insert(index, Positions[index]);

                for (var i = index + 1; i < Positions.Count; ++i)
                    Positions[i] += size;
            }
        }

        public void Remove(int index)
        {
            var size = Sizes[index];
            TotalSize -= size;
            Sizes.RemoveAt(index);
            Positions.RemoveAt(index);

            for (var i = index; i < Positions.Count; ++i)
                Positions[i] -= size;
        }

        public Point GetVisibleCellRange(int screenStart, int screenEnd)
        {
            var range = new Point(int.MaxValue, int.MinValue);
            if (Sizes.Count == 0)
                return range;

            for (var i = 0; i < Sizes.Count; ++i)
            {
                var size = Sizes[i];
                var position = Positions[i];
                var isVisible = !(position + size < screenStart || position > screenEnd);
                if (isVisible)
                {
                    range.x = Mathf.Min(range.x, i);
                    range.y = i;
                }

                if (position + size > screenEnd)
                    break;
            }

            return range;
        }
    }
}