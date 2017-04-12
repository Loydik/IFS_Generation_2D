using System;
using System.Collections.Generic;

namespace IFS_Thesis.Utils
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Shuffles the collection
        /// </summary>
        public static void Shuffle<T>(this IList<T> list, Random randomGen)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                int k = randomGen.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
