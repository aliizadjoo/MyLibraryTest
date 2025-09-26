using Microsoft.Data.SqlClient.DataClassification;
using MyLibraryTest.DTOs;
using MyLibraryTest.Entities;
using MyLibraryTest.Enums;
using MyLibraryTest.Exceptions;
using MyLibraryTest.Services;

Service service = new Service();

while (true)
{
    if (LocalStorage.LogInUser == null)
    {
        Console.WriteLine("1.Login or 2.Register ? .Please select an option.");
        try
        {
            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:

                    Console.WriteLine("please enter username");
                    string username = Console.ReadLine();
                    Console.WriteLine("please enter pass");
                    string pass = Console.ReadLine();
                    try
                    {
                        service.LogIn(username, pass);
                        Console.WriteLine("Log in was successful.");
                    }
                    catch (LoginFailedException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;

                case 2:
                    Console.WriteLine("please enter username");
                    username = Console.ReadLine();
                    Console.WriteLine("please enter pass");
                    string password = Console.ReadLine();
                    Console.WriteLine("please enter Role");
                    string role = Console.ReadLine();
                    try
                    {
                        RoleEnum roleEnum = (RoleEnum)Enum.Parse(typeof(RoleEnum), role);
                        service.Register(username, password, roleEnum);
                        Console.WriteLine("Registration was successful.");
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine($"The entered role '{role}' is not valid. Please enter 'Admin' or 'User'.");
                    }
                    catch (UserAlreadyExistsException e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    break;

            }
        }
        catch (FormatException)
        {
            Console.WriteLine("please enter number :1.Login or 2.Register");
        }


    }
    else
    {
        User userLogIn = LocalStorage.LogInUser;
        switch (userLogIn.Role)
        {
            case RoleEnum.User:
                ShowMenuUser();
                try
                {
                    int choice = int.Parse(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:

                            ShowAllBooksAndCategories();

                            break;
                        case 2:
                            ShowAllBooks();
                            Console.WriteLine("Please select the desired book ID.");
                            try
                            {
                                int bookIdSelected = int.Parse(Console.ReadLine());
                                service.BorrowBook(bookIdSelected);
                                Console.WriteLine("The book has been successfully loaned to you.");

                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("invalid BookId");
                            }
                            catch (UserNotLoggedInException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            catch (BookNotFoundException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        case 3:
                            ShowMyBorrowedBooks();
                            break;
                        case 4:
                            ShowAllBooks();
                            Console.WriteLine("Please select the desired book ID.");
                            try
                            {
                                int bookIdSelected = int.Parse(Console.ReadLine());
                                Console.WriteLine("please enter rate 1 until 5");
                                int rate = int.Parse(Console.ReadLine());
                                Console.WriteLine("please enter comment");
                                string comment = Console.ReadLine();
                                service.SubmitReview(bookIdSelected, rate, comment);
                                Console.WriteLine("rate is successfully register");
                            }
                            catch (BookNotFoundException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            catch (UserNotLoggedInException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("invalid BookId");
                            }
                            catch (InvalidRatingException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        case 5:
                            ShowAllReviews();
                            Console.WriteLine("Please select the desired review ID. ");
                            try
                            {
                                int reviewId = int.Parse(Console.ReadLine());
                                CommentManagementMenu();
                                Console.WriteLine("1.edit or 2.Remove");
                                try
                                {
                                    int option = int.Parse(Console.ReadLine());
                                    switch (option)
                                    {
                                        case 1:

                                            Console.WriteLine("please enter comment");
                                            string comment = Console.ReadLine();
                                            Console.WriteLine("please enter rating");
                                            int rating = int.Parse(Console.ReadLine());
                                            service.EditReview(reviewId , comment , rating);
                                            Console.WriteLine("Review successfully edited.");

                                            break;
                                        case 2:
                                            service.RemoveReview(reviewId);
                                            Console.WriteLine("Review successfully removed.");
                                            break;

                                        default:
                                            Console.WriteLine("invalid invlaid option please enter 1.edit or 2.Remove");
                                            break;
                                    }
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("invlaid option please enter number ,1.edit or 2.Remove");
                                }
                                catch(UnauthorizedAccessException e) 
                                {
                                    Console.WriteLine(e.Message);
                                }
                                catch(InvalidRatingException e) 
                                {
                                    Console.WriteLine(e.Message);
                                }

                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("invalid reviewID ");
                            }
                            break;
                        case 6:
                            LocalStorage.LogOutUser();
                            break;

                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("invalid option please enter 1 or 2 or 3 or 4");
                }

                break;

            case RoleEnum.Admin:
                ShowMenuAdmin();
                try
                {
                    int choice = int.Parse(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("please enter CategoryName:");
                            string CategoryName = Console.ReadLine();
                            try
                            {
                                service.AddCategory(CategoryName);
                                Console.WriteLine("Category added successfully.");
                            }
                            catch (CategoryAlreadyExistsException e)
                            {
                                Console.WriteLine(e.Message);
                            }

                            break;
                        case 2:
                            Console.WriteLine("please enter title:");
                            string title = Console.ReadLine();
                            Console.WriteLine("Please select the desired category ID:");
                            ShowAllCategory();
                            try
                            {
                                int categoryId = int.Parse(Console.ReadLine());
                                service.AddBook(title, categoryId);
                                Console.WriteLine("Book added successfully.");
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("The entered category ID is invalid.");
                            }
                            catch (CategoryNotFoundException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        case 3:
                            ShowAllBooksAndCategories();
                            break;
                        case 4:
                            ManagePendingReviews();
                            break;
                            
                        case 5:
                            LocalStorage.LogOutUser();
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("invalid option please enter 1 or 2 or 3 or 4");
                }
                break;

        }
    }
}
void ShowMenuUser()
{
    Console.WriteLine("Please select an option.");
    Console.WriteLine("1.View categories and books");
    Console.WriteLine("2.Borrow a book");
    Console.WriteLine("3.View my borrowed books ");
    Console.WriteLine("4.Submit a review for a book");
    Console.WriteLine("5.Manage my reviews");
    Console.WriteLine("6.logout");
}
void ShowMenuAdmin()
{
    Console.WriteLine("Please select an option.");
    Console.WriteLine("1.Add a new category");
    Console.WriteLine("2.Add a new book to a category");
    Console.WriteLine("3.View all books and categories");
    Console.WriteLine("4.Manage reviews");
    Console.WriteLine("5.logout");
}
void ShowAllBooksAndCategories()
{
  
    ShowAllBooks();

    Console.WriteLine("Please enter the ID of the book you want to see the details for:");
    try
    {
        int bookId = int.Parse(Console.ReadLine());
        ShowBookDetails(bookId);
    }
    catch (FormatException)
    {
        Console.WriteLine("Invalid ID format. Please enter a number.");
    }
}
void ShowAllCategory()
{
    AllBooksAndCategoriesDto AllBooksAndCategories = service.GetAllBooksAndCategories();
    if (AllBooksAndCategories.Categories.Count == 0)
    {
        Console.WriteLine("No category defined.");
    }
    foreach (var category in AllBooksAndCategories.Categories)
    {
        Console.WriteLine($"CategoryId:{category.Id} , {category.Name}");
    }
}

void ShowAllBooks()
{
    AllBooksAndCategoriesDto AllBooksAndCategories = service.GetAllBooksAndCategories();
    foreach (var book in AllBooksAndCategories.Books)
    {
        Console.WriteLine($"bookID:{book.Id} , title:{book.Title} , category: {book.Category.Name}");
    }
}
void ShowAllReviews()
{

    List<Review> reviews = service.GetMyReviews();

    if (reviews.Count == 0)
    {
        Console.WriteLine("You have not posted a comment yet.");
        return;
    }

    Console.WriteLine("--- Comments posted by you ---");
    foreach (var review in reviews)
    {

        Console.WriteLine($"Review ID: {review.Id} | Book: {review.Book.Title} | Rating: {review.Rating}");
        Console.WriteLine($"   Comment: {review.Comment}");
        Console.WriteLine("---------------------------------");
    }
}
void ShowMyBorrowedBooks()
{
    List<BorrowedBook> borrowedBooks = service.GetMyBorrowedBooks();
    foreach (var borrowedBook in borrowedBooks)
    {
        Console.WriteLine($"bookId:{borrowedBook.BookId} , title:{borrowedBook.Book.Title} , " +
            $"Category: {borrowedBook.Book.Category.Name} , BorrowDate:{borrowedBook.BorrowDate}");
    }
}

void ShowBookDetails(int bookId)
{
    try
    {
        
        BookDetailsDto bookDetails = service.GetBookDetails(bookId);       
        Console.WriteLine("\n--- Book Details ---");
        Console.WriteLine($"Title: {bookDetails.Title}");
        Console.WriteLine($"Category: {bookDetails.Category.Name}");     
        Console.WriteLine($"Average Rating: {Math.Round(bookDetails.AverageRating, 2)} / 5");
        Console.WriteLine("--------------------");

        Console.WriteLine("\n--- Approved Reviews ---");
        if (bookDetails.ApprovedReviews.Count == 0)
        {
            Console.WriteLine("No approved reviews for this book yet.");
        }
        else
        {
            foreach (var review in bookDetails.ApprovedReviews)
            {
                Console.WriteLine($"User: {review.User.Username} | Rating: {review.Rating}");
                Console.WriteLine($"   Comment: {review.Comment}");
                Console.WriteLine();
            }
        }
        Console.WriteLine("------------------------");
    }
    catch (BookNotFoundException e)
    {
        Console.WriteLine(e.Message);
    }
    catch (Exception e)
    {
        Console.WriteLine("An unexpected error occurred: " + e.Message);
    }
}

void CommentManagementMenu()
{
    Console.WriteLine("1.Edit");
    Console.WriteLine("2.Remove");
}
void ManagePendingReviews()
{
    
    List<Review> pendingReviews = service.GetPendingReviews();

    if (pendingReviews.Count == 0)
    {
        Console.WriteLine("\nThere are no pending reviews to manage.");
        return;
    }

   
    Console.WriteLine("\n--- Pending Reviews ---");
    foreach (var review in pendingReviews)
    {
        Console.WriteLine($"Review ID: {review.Id} | Book: '{review.Book.Title}' | User: {review.User.Username}");
        Console.WriteLine($"   Rating: {review.Rating} | Comment: {review.Comment}");
        Console.WriteLine("---------------------------------");
    }

    try
    {
      
        Console.Write("\nPlease enter the ID of the review you want to manage: ");
        int reviewId = int.Parse(Console.ReadLine());

        
        Console.WriteLine("1. Approve | 2. Reject");
        int choice = int.Parse(Console.ReadLine());

        if (choice == 1)
        {
            service.ApproveReview(reviewId);
            Console.WriteLine("Review successfully approved.");
        }
        else if (choice == 2)
        {
            service.RejectReview(reviewId);
            Console.WriteLine("Review successfully rejected and removed.");
        }
        else
        {
            Console.WriteLine("Invalid option.");
        }
    }
    catch (FormatException)
    {
        Console.WriteLine("Invalid input. Please enter a number.");
    }
    catch (ReviewNotFoundException e)
    {
        Console.WriteLine(e.Message);
    }
    catch (Exception e)
    {
        Console.WriteLine("An unexpected error occurred: " + e.Message);
    }
}

