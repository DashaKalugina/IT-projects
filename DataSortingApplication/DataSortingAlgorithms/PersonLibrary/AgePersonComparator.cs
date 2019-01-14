using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonLibrary
{
    /// <summary>
    /// Класс реализует интерфейс IComparer и позволяет сравнивать объекты класса Person по полю Age
    /// </summary>
    public class AgePersonComparator : IComparer
    {
        public bool DescendingOrder { get; set; }
        public int Compare(object x, object y)
        {
            var person1 = (Person)x;
            var person2 = (Person)y;
            return person1.Age.CompareTo(person2.Age) * (DescendingOrder ? -1 : 1);
        }
    }
}
