using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSortingAlgorithms
{
    static class ArrayExtentions
    {
        /// <summary>
        /// Метод расширения (меняет местами i-ый и j-ый элементы массива произвольного типа T)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public static void Swap<T>(this T[] array, int i, int j)
        {
            var buf = array[i];
            array[i] = array[j];
            array[j] = buf;
        }
    }
}
