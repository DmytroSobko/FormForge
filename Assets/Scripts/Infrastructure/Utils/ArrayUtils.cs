using System.Linq;

namespace FormForge.Utils
{
    public static class ArrayUtils
    {
        /// <summary>
        /// Compares two arrays to see if they contain the same values
        /// </summary>
        /// <param name="array1">First array</param>
        /// <param name="array2">Second array</param>
        /// <typeparam name="T">The type of array being compared</typeparam>
        /// <returns>True if both arrays contain the same values</returns>
        public static bool AreArraysEqual<T>(T[] array1, T[] array2)
        {
            if (array1 == null || array2 == null)
                return false;

            if (array1.Length != array2.Length)
                return false;

            var sortedArray1 = array1.OrderBy(x => x).ToArray();
            var sortedArray2 = array2.OrderBy(x => x).ToArray();

            return sortedArray1.SequenceEqual(sortedArray2);
        }
    }
}