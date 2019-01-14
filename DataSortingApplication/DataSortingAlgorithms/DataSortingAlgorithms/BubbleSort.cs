using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSortingAlgorithms
{
    public static class BubbleSort
    {
        /// <summary>
        /// Метод, реализующий алгоритм сортировки "пузырьком"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"> Массив элементов произвольного типа T</param>
        /// <param name="comparator"> Указатель на метод-компаратор, реализующий способ сортировки для конкретного типа данных</param>
        /// <returns> Отсортированный массив </returns>
        public static T[] Sort<T>(T[] array, Func<T, T, int> comparator)
        {
            if (array.Length == 0)
                return null;
            if (array.Length == 1)
                return array;
            for (int i = array.Length - 1; i >= 0; i--)
            {
                for (int j = 1; j <= i; j++)
                {
                    if (comparator(array[j - 1], array[j]) > 0)
                    {
                        array.Swap(j, j - 1);
                    }
                }
            }
            return array;
        }
    }
}
