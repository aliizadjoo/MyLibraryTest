using MyLibraryTest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibraryTest.DTOs
{
    public class BookDetailsDto
    {
        public List<Review> ApprovedReviews { get; set; }
        public double AverageRating { get; set; }

        public string Title { get; set; }

        public Category Category { get; set; }
    }
}
