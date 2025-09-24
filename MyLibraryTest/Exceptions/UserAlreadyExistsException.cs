using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibraryTest.Exceptions
{
    public class UserAlreadyExistsException : ValidationException
    {
        public UserAlreadyExistsException(string message) : base(message)
        {
        }
    }
}
