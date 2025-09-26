using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibraryTest.Exceptions
{
    public class InvalidRatingException : ValidationException
    {
        public InvalidRatingException(string message) : base(message)
        {
        }
    }
}
