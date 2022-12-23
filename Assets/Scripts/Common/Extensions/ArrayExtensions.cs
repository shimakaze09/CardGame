using System.Linq;

namespace TheLiquidFire.Extensions
{
    public static class ArrayExtensions
    {
        public static T Random<T>(this T[] array)
        {
            var index = UnityEngine.Random.Range(0, array.Length);
            return array[index];
        }

        public static T First<T>(this T[] array)
        {
            return array[0];
        }

        public static T Last<T>(this T[] array)
        {
            return array[^1];
        }

        public static bool Matches<T>(this T[] array1, T[] array2)
        {
            if (array1 == null || array2 == null)
                return false;
            if (array1.Length != array2.Length)
                return false;

            return !array1.Where((t, i) => !t.Equals(array2[i])).Any();
        }
    }
}