using System.Collections.Generic;
using UnityEngine;

namespace TheLiquidFire.Extensions
{
    public static class ListExtensions
    {
        public static T Random<T>(this List<T> list)
        {
            var index = UnityEngine.Random.Range(0, list.Count);
            return list[index];
        }

        public static T First<T>(this List<T> list)
        {
            return list[0];
        }

        public static T Last<T>(this List<T> list)
        {
            return list[^1];
        }

        public static T Draw<T>(this List<T> list)
        {
            if (list.Count == 0)
                return default;

            var index = UnityEngine.Random.Range(0, list.Count);
            var result = list[index];
            list.RemoveAt(index);
            return result;
        }

        public static List<T> Draw<T>(this List<T> list, int count)
        {
            var resultCount = Mathf.Min(count, list.Count);
            var result = new List<T>(resultCount);
            for (var i = 0; i < resultCount; i++)
            {
                var item = list.Draw();
                result.Add(item);
            }

            return result;
        }
    }
}