using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollection
{
    public class PersonData
    {
        public string LibraryCard { get; set; }     //Номер читательского билета
        public string Name { get; set; }            //ФИО
        public string DateOfBirth { get; set; }   //Дата рождения
        public string Address { get; set; }         //Адрес 
        public string Group { get; set; }
        public string PhoneNumber { get; set; }
        public List<Book> Books { get; set; }       //Список книг на руках
    }

    public class Book
    {
        public string Title { get; set; }       //название книги
        public string Author { get; set; }      //автор
        public int YearOfIssue { get; set; }    //год издания
        public bool Reissue { get; set; }       //повторное издание или книга напечатана впервые
        public string Genre { get; set; }       //жанр
        public string Isbn { get; set; }        //номер ISBN

        public override string ToString()
        {
            return string.Format("{0} / {1} ", Title, Author);
        }
    }

}
