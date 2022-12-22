using System;
using System.Collections.Generic;

namespace TheLiquidFire.Extensions
{
    public static class SetExtensions
    {
        public static List<T> ToSortedList<T>(this HashSet<T> set)
        {
            var array = new T[set.Count];
            set.CopyTo(array);

            var list = new List<T>(array);
            list.Sort();
            return list;
        }

        public static List<T> ToSortedList<T>(this HashSet<T> set, IComparer<T> comparer)
        {
            var array = new T[set.Count];
            set.CopyTo(array);

            var list = new List<T>(array);
            list.Sort(comparer);
            return list;
        }

        public static List<T> ToSortedList<T>(this HashSet<T> set, Comparison<T> comparison)
        {
            var array = new T[set.Count];
            set.CopyTo(array);

            var list = new List<T>(array);
            list.Sort(comparison);
            return list;
        }

        public static List<T> ToSortedList<T>(this HashSet<T> set, int index, int count, IComparer<T> comparer)
        {
            var array = new T[set.Count];
            set.CopyTo(array);

            var list = new List<T>(array);
            list.Sort(index, count, comparer);
            return list;
        }
    }
}