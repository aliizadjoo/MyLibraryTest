using Microsoft.EntityFrameworkCore;
using MyLibraryTest.DataAccess;
using MyLibraryTest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibraryTest.Services
{
    public class Repository
    {
        public User? GetUserByUserName(string username)
        {
            using (var context = new AppDbContext())
            {

                return context.Users.FirstOrDefault(u => u.Username.ToLower() == username.ToLower());
            }
        }
        public void AddUser(User user)
        {
            using (var context = new AppDbContext())
            {

                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public List<Book> GetAllBooks()
        {
            using (var _context = new AppDbContext())
            {
                return _context.Books.Include(b => b.Category).ToList();

            }
        }

        public Book? GetBookById(int bookId)
        {
            using (var _context = new AppDbContext())
            {
                return _context.Books.FirstOrDefault(b => b.Id == bookId);
            }

        }

        public List<Category> GetAllCategories()
        {
            using (var _context = new AppDbContext())
            {
                return _context.Categories.ToList();
            }
        }

        public void AddBorrowedBook(BorrowedBook borrowedBook)
        {
            using (var _context = new AppDbContext())
            {
                _context.BorrowedBooks.Add(borrowedBook);
                _context.SaveChanges();


            }

        }

        public List<BorrowedBook> GetBorrowedBooksByUserId(int userId)
        {
            using (var _context = new AppDbContext())
            {
                return _context.BorrowedBooks.Where(bb => bb.UserId == userId).Include(bb => bb.Book)
                     .ThenInclude(b => b.Category).ToList();
            }
        }

        public void AddCategory(Category category)
        {
            using (var _context = new AppDbContext())
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
            }
        }
        public Category? GetCategoryByName(string name) 
        {
            using ( var _context = new AppDbContext())
            {
                return _context.Categories.FirstOrDefault(c => c.Name == name);
            }
        }
        public Category? GetCategoryById(int categoryId) 
        {
           using( var _context = new AppDbContext()) 
           {
                return _context.Categories.FirstOrDefault(c=>c.Id==categoryId); 
           }
        }

        public void AddBook(Book book) 
        {
            using( var _context = new AppDbContext()) 
            {
               _context.Books.Add(book);
               _context.SaveChanges();
            }
        
        }

        public void AddReview(Review review) 
        {
          using(var _context = new AppDbContext())
          {
             _context.Reviews.Add(review);
             _context.SaveChanges();
          }
        }

        public void UpdateReview(Review review) 
        {
            using (var _context = new AppDbContext()) 
            {
                _context.Reviews.Update(review);
                _context.SaveChanges();              
            }
        }

        public void RemoveReview(Review review) 
        {
           using(var _context = new AppDbContext()) 
           {
              _context.Reviews.Remove(review);
              _context.SaveChanges();
           } 
        }

        public List<Review> GetReviewsByUserId(int userId) 
        {
            using (var _context = new AppDbContext()) 
            {
               return _context.Reviews.Where(r=>r.UserId==userId)
                    .Include(r=>r.Book).ToList();   

            }


        } 

        public Review? GetReviewsByReviewId(int reviewId) 
        {
           using(var _context = new AppDbContext()) 
           {
               return _context.Reviews.FirstOrDefault(r => r.Id == reviewId);
           }
        }

        public Book? GetBookWithApprovedReviews(int bookId)
        {
            using (var _context = new AppDbContext())
            {
                return _context.Books
                    .Include(b => b.Category) 
                    .Include(b => b.Reviews)
                        .ThenInclude(r => r.User) 
                    .FirstOrDefault(b => b.Id == bookId);
            }
        }

        public List<Review> GetPendingReviews()
        {
            using (var _context = new AppDbContext())
            {
                
                return _context.Reviews
                    .Where(r => !r.IsApproved)
                    .Include(r => r.Book)      
                    .Include(r => r.User)     
                    .ToList();
            }
        }

        public Review GetReviewById(int reviewId)
        {
            using (var _context = new AppDbContext())
            {
                return _context.Reviews.FirstOrDefault(r => r.Id == reviewId);
            }
        }
    }
}
