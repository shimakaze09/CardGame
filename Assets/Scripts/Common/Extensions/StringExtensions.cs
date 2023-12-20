using UnityEngine;
using System.Collections;
using System;

namespace TheLiquidFire.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string target, StringComparison comparison)
        {
            var index = source.IndexOf(target, comparison);
            return index >= 0;
        }
    }
}