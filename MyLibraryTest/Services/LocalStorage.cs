using MyLibraryTest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibraryTest.Services
{
    public static class LocalStorage
    {
        public static User LogInUser { get; private set; }

        public static void LogOutUser() 
        {
            LogInUser = null;
        }

        public static void LogIn(User user) 
        {
            LogInUser = user;
        }

    }
}
