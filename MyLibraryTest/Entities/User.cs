using MyLibraryTest.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibraryTest.Entities
{
    public class User 
    {
        private User()
        {
            
        }
        public User(string username , string password , RoleEnum role )
        {
            Username = username;
            Password = password;
            Role = role;
        }
        public int Id { get; set; }
        public string Username { get; set; }
        private string Password { get; set; }

        public List<Review> Reviews { get; set; } = [];
        public RoleEnum Role { get; set; }
        public List<BorrowedBook> BorrowedBooks { get; set; } = [];

        public bool CheckPass(string pass) 
        {
            if (Password==pass)
            {
                return true;
            }
            return false;
        }

    }
}
