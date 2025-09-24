using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibraryTest.Exceptions
{
    public class CategoryNotFoundException : ValidationException
    {
        public CategoryNotFoundException(string message) : base(message)
        {
        }
    }
}
