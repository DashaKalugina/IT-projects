using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Comparators;
using PersonLibrary;

namespace DataSortingAlgorithms.Tests
{
    [TestClass]
    public class DataSortingTest
    {
        /// <summary>
        /// Сортировка пустого массива
        /// </summary>
        [TestMethod]
        public void EmptyIntArrayTest()
        {
            var array = new int[] {};
            var comparator = new GenericComparator { DescendingOrder = false };
            var actual = BubbleSort.Sort(array, comparator.CompareObjects);
            CollectionAssert.AreEqual(null, actual);
        }

        /// <summary>
        /// Сортировка массива элементов произвольного типа T 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"> Сортируемый массив </param>
        /// <param name="expected"> Ожидаемый результат сортировки</param>
        /// <param name="descendingOrder">Сортировка по возрастанию или убыванию</param>
        public void SortingTest<T>(T[] array, T[] expected, bool descendingOrder)
        {
            var comparator = new GenericComparator { DescendingOrder = descendingOrder };
            var actual = BubbleSort.Sort(array, comparator.CompareObjects);

            Assert.AreEqual(expected.Length, actual.Length);
            CollectionAssert.AreEqual(expected, actual);
        }
        
        
        [TestMethod]
        public void OneElementArray()
        {
            SortingTest(new int[] { 1 }, new int[] { 1 }, false);
            SortingTest(new double[] { 2.5 }, new double[] { 2.5 }, true);
            SortingTest(new string[] { "тест" }, new string[] { "тест" }, true);
        }

        [TestMethod]
        public void AscendingSortedIntArray()
        {
            SortingTest(new int[] { 1, 2, 3, 4, 5 }, new int[] { 1, 2, 3, 4, 5 }, false);
            SortingTest(new int[] { 1, 2, 3, 4, 5 }, new int[] { 5, 4, 3, 2, 1 }, true);
        }

        [TestMethod]
        public void DescendingSortedIntArray()
        {
            SortingTest(new int[] { 5, 4, 3, 2, 1 }, new int[] { 1, 2, 3, 4, 5 }, false);
            SortingTest(new int[] { 5, 4, 3, 2, 1 }, new int[] { 5, 4, 3, 2, 1 }, true);
        }

        [TestMethod]
        public void IntArray()
        {
            SortingTest(new int[] { 4, 5, 2, 3, 1 }, new int[] { 1, 2, 3, 4, 5 }, false);
            SortingTest(new int[] { 4, 5, 2, 3, 1 }, new int[] { 5, 4, 3, 2, 1 }, true);
        }

        [TestMethod]
        public void DoubleArray()
        {
            SortingTest(new double[] { 2.7, 6.4, 3.12, 8.0, 0, 2.5 }, new double[] { 0, 2.5, 2.7, 3.12, 6.4, 8.0 }, false);
            SortingTest(new double[] { 2.7, 6.4, 3.12, 8.0, 0, 2.5 }, new double[] { 8.0, 6.4, 3.12, 2.7, 2.5, 0}, true);
        }

        [TestMethod]
        public void StringArray()
        {
            SortingTest(new string[] { "b", "a", "k", "g"}, new string[] { "a", "b", "g", "k" }, false);
            SortingTest(new string[] { "b", "a", "k", "g" }, new string[] { "k", "g", "b", "a" }, true);
            SortingTest(new string[] { "aa", "a", "aaaa", "aaa" }, new string[] { "a", "aa", "aaa", "aaaa" }, false);
        }
       
        /// <summary>
        /// Сортировка массива из элементов нового типа Person по имени Name (в алфавитном порядке)
        /// </summary>
        /// <param name="array"></param>
        /// <param name="expected"></param>
        /// <param name="descendingOrder"></param>
        public void NamePersonSorting(Person[] array, Person[] expected, bool descendingOrder)
        {
            var comparator = new NamePersonComparator { DescendingOrder = descendingOrder };
            var actual = BubbleSort.Sort(array, comparator.Compare);

            Check(expected, actual);
        }

        /// <summary>
        /// Сортировка массива из элементов нового типа Person по возрасту Age
        /// </summary>
        /// <param name="array"></param>
        /// <param name="expected"></param>
        /// <param name="descendingOrder"></param>
        public void AgePersonSorting(Person[] array, Person[] expected, bool descendingOrder)
        {
            var comparator = new AgePersonComparator { DescendingOrder = descendingOrder };
            var actual = BubbleSort.Sort(array, comparator.Compare);

            Check(expected, actual);
        }

        /// <summary>
        /// Проверка сортировки массива из объектов класса Person
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public void Check(Person[] expected, Person[] actual)
        {
            Assert.AreEqual(expected.Length, actual.Length);

            for (int i = 0; i < actual.Length; i++)
            {
                Assert.AreEqual(expected[i].Name, actual[i].Name);
                Assert.AreEqual(expected[i].Age, actual[i].Age);
            }
        }

        [TestMethod]
        public void PersonArray()
        {
            var array = new Person[]
            {
                new Person{Name="Иван",Age=26},
                new Person{Name="Федор",Age=10},
                new Person{Name="Анна",Age=15},
                new Person{Name="Ольга",Age=40}
            };
            var byNameExpexted = new Person[]
            {
                new Person{Name="Анна",Age=15},
                new Person{Name="Иван",Age=26},
                new Person{Name="Ольга",Age=40},
                new Person{Name="Федор",Age=10}
            };

            NamePersonSorting(array, byNameExpexted, false);

            var byAgeExpected = new Person[]
            {
                new Person{Name="Ольга",Age=40},
                new Person{Name="Иван",Age=26},
                new Person{Name="Анна",Age=15},
                new Person{Name="Федор",Age=10}
            };

            AgePersonSorting(array, byAgeExpected, true);
        }
    }
}
