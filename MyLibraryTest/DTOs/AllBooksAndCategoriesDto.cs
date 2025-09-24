using MyLibraryTest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibraryTest.DTOs
{
    public class AllBooksAndCategoriesDto
    {
        public List<Book> Books { get; set; } = [];
        public List<Category> Categories { get; set; }= [];
    }
}
