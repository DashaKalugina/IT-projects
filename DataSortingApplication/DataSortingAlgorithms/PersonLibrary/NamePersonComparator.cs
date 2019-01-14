using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonLibrary
{
    /// <summary>
    /// Класс реализует интерфейс IComparer и позволяет сравнивать объекты класса Person по полю Name
    /// </summary>
    public class NamePersonComparator : IComparer
    {
        public bool DescendingOrder { get; set; }
        public int Compare(object x, object y)
        {
            var person1 = (Person)x;
            var person2 = (Person)y;
            return person1.Name.CompareTo(person2.Name) * (DescendingOrder ? -1 : 1);
        }
    }
}
