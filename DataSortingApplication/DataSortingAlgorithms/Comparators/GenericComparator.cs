using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comparators
{
    public class GenericComparator
    {
        // сортировка по возрастанию или убыванию
        public bool DescendingOrder { get; set; }

        /// <summary>
        /// Дженерик-метод сравнения двух объектов типа T, реализующих интерфейс IComparable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public int CompareObjects<T>(T obj1, T obj2)
        {
            var comparableObj1 = (IComparable)obj1;
            return comparableObj1.CompareTo(obj2) * (DescendingOrder ? -1 : 1);
        }
    }
}
