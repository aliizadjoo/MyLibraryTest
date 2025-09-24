using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibraryTest.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public Category Category { get; set; }

        public int CategoryId { get; set; }
    }
}
