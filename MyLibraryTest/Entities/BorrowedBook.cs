using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibraryTest.Entities
{
    public class BorrowedBook
    {
        public int Id { get; set; }
        public Book Book { get; set; }

        public User User { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }

        public DateTime BorrowDate { get; set; }


    }
}
