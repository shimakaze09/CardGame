﻿using TheLiquidFire.DataTypes;
using UnityEngine;

namespace TheLiquidFire.UI
{
    public interface IFlow
    {
        void ConfigureCell(TableViewCell cell, int index);
        Point GetVisibleCellRange();
        Vector2 GetCellOffset(int index);
        Vector2 GetCellOffset(int index, int padding);
    }
}