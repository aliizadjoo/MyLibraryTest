using MyLibraryTest.DTOs;
using MyLibraryTest.Entities;
using MyLibraryTest.Enums;
using MyLibraryTest.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace MyLibraryTest.Services
{
    public class Service
    {
        Repository repository = new Repository();
        public void Register(string username, string password, RoleEnum role) 
        {
            User userFromDb= repository.GetUserByUserName(username);
            if (userFromDb != null) 
            {
                throw new UserAlreadyExistsException("A user has already registered with this username.");
            }
            User user = new User(username , password  , role);      
            repository.AddUser(user);
        }
        public void LogIn(string username, string password)
        {
           
            User? userFromDb = repository.GetUserByUserName(username);

          
            if (userFromDb == null)
            {
                
                throw new LoginFailedException("The username or password is incorrect..");
            }

          
            bool isPasswordCorrect = userFromDb.CheckPass(password);
            if (isPasswordCorrect == false)
            {
                
                throw new LoginFailedException("The username or password is incorrect..");
            }

            LocalStorage.LogIn(userFromDb);
        }

        public AllBooksAndCategoriesDto GetAllBooksAndCategories() 
        {
            List<Category> categories=repository.GetAllCategories();
            List<Book> books = repository.GetAllBooks();
            var allBooksAndCategories = new AllBooksAndCategoriesDto
            {
                Categories = categories,
                Books = books
            };
            return allBooksAndCategories;
        }
        public void BorrowBook(int bookId)
        {
            
            Book? book = repository.GetBookById(bookId);
            if (book == null)
            {
                throw new BookNotFoundException("No book with this ID was found.");
            }

            if (LocalStorage.LogInUser == null)
            {
                throw new UserNotLoggedInException("The user is not logged in.");
            }

            var borrowedBook = new BorrowedBook
            {
                BookId = book.Id, 
                UserId = LocalStorage.LogInUser.Id, 
                BorrowDate = DateTime.Now
            };

            
            repository.AddBorrowedBook(borrowedBook);
        }

        public List<BorrowedBook> GetMyBorrowedBooks() 
        {
            return repository.GetBorrowedBooksByUserId(LocalStorage.LogInUser.Id);
        
        }

        public void AddCategory(string name) 
        {
            Category category=repository.GetCategoryByName(name);
            if (category!=null)
            {
                throw new CategoryAlreadyExistsException("A category with this name has already been defined.");
            }
            var newCategory = new Category
            {
                Name = name,
            };
            repository.AddCategory(newCategory);        
        }

        public void AddBook(string title , int categoryId) 
        {
            Category  category=repository.GetCategoryById(categoryId);
            if (category == null) 
            {
                throw new CategoryNotFoundException("No category with this ID was found.");
            }
            var book = new Book 
            {
              Title=title,
              CategoryId=categoryId

            };
             repository.AddBook(book);
        }

        public void SubmitReview(int bookId , int rating , string comment) 
        {
           Book book= repository.GetBookById(bookId);
            if(book == null) 
            {
                throw new BookNotFoundException("No book with this ID was found");
            }
            User user= LocalStorage.LogInUser;
            if (user==null)
            {
                throw new UserNotLoggedInException("The user is not logged in.");
            }
            if (rating>5||rating<1)
            {
                throw new InvalidRatingException("The score entered is invalid. The score must be between one and five.");
            }
            var review = new Review
            {
                UserId=user.Id,
                BookId=bookId,
                Comment=comment,
                Rating=rating,
                CreatedAt=DateTime.Now,
                IsApproved=false
            };
            repository.AddReview(review);
        }

        public List<Review> GetMyReviews() 
        {
            if (LocalStorage.LogInUser == null)
            {
                throw new UserNotLoggedInException("The user is not logged in.");
            }
            return  repository.GetReviewsByUserId(LocalStorage.LogInUser.Id);
        }

        public void EditReview(int reviewId , string comment , int rating ) 
        {
           Review review=repository.GetReviewsByReviewId(reviewId);
            if (review==null)
            {
                throw new ReviewNotFoundException("No Review with this ID was found");
            }
            if (review.UserId!=LocalStorage.LogInUser.Id)
            {
                throw new UnauthorizedAccessException("Access denied. You do not have permission to edit this review.");
            }
            if (rating > 5 || rating < 1)
            {
                throw new InvalidRatingException("The score entered is invalid. The score must be between one and five.");
            }
            review.Comment = comment;
            review.Rating = rating;
            repository.UpdateReview(review);
        }

        public void RemoveReview(int reviewId) 
        {
            Review review = repository.GetReviewsByReviewId(reviewId);
            if (review == null)
            {
                throw new ReviewNotFoundException("No Review with this ID was found");
            }
            if (review.UserId != LocalStorage.LogInUser.Id)
            {
                throw new UnauthorizedAccessException("Access denied. You do not have permission to edit this review.");
            }
            repository.RemoveReview(review);
        }

        public BookDetailsDto GetBookDetails(int bookId)
        {
            
            Book book = repository.GetBookWithApprovedReviews(bookId);

            if (book == null)
            {
                throw new BookNotFoundException("No book with this ID was found.");
            }          
            var approvedReviews = book.Reviews.Where(r => r.IsApproved).ToList();
         
            double avgRating = 0;
            if (approvedReviews.Any()) 
            {
                avgRating = approvedReviews.Average(r => r.Rating);
            }           
            var bookDetailsDto = new BookDetailsDto
            {
                Title = book.Title,
                Category = book.Category,
                ApprovedReviews = approvedReviews,
                AverageRating = avgRating
            };

            return bookDetailsDto;
        }

        public List<Review> GetPendingReviews()
        {
            return repository.GetPendingReviews();
        }

        public void ApproveReview(int reviewId)
        {
            Review review = repository.GetReviewById(reviewId);
            if (review == null)
            {
                throw new ReviewNotFoundException("No review with this ID was found.");
            }

            review.IsApproved = true;
            repository.UpdateReview(review);
        }

        public void RejectReview(int reviewId)
        {
            Review review = repository.GetReviewById(reviewId);
            if (review == null)
            {
                throw new ReviewNotFoundException("No review with this ID was found.");
            }

            repository.RemoveReview(review);
        }
    }
}
