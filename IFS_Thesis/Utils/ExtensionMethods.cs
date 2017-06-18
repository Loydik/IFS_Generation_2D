using System;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// Clones the collection
        /// </summary>
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}
