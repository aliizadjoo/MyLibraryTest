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
                            catch(CategoryAlreadyExistsException e) 
                            {
                                Console.WriteLine(e.Message);
                            }
                           
                            break;
                        case 2:
                            Console.WriteLine("please enter title:");
                            string title =Console.ReadLine();
                            Console.WriteLine("Please select the desired category ID:");
                            ShowAllCategory();
                            try 
                            {
                                int categoryId=int.Parse(Console.ReadLine());
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
    Console.WriteLine("4.logout");
}
void ShowMenuAdmin()
{
    Console.WriteLine("Please select an option.");
    Console.WriteLine("1.Add a new category");
    Console.WriteLine("2.Add a new book to a category");
    Console.WriteLine("3.View all books and categories");
    Console.WriteLine("4.logout");
}
void ShowAllBooksAndCategories()
{
    AllBooksAndCategoriesDto AllBooksAndCategories = service.GetAllBooksAndCategories();
    ShowAllCategory();
    if (AllBooksAndCategories.Books.Count == 0)
    {
        Console.WriteLine("The library has no books.");
    }
    foreach (var book in AllBooksAndCategories.Books)
    {
        Console.WriteLine($"bookID:{book.Id} , title:{book.Title} , category: {book.Category.Name}");
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

void ShowMyBorrowedBooks()
{
    List<BorrowedBook> borrowedBooks = service.GetMyBorrowedBooks();
    foreach (var borrowedBook in borrowedBooks)
    {
        Console.WriteLine($"bookId:{borrowedBook.BookId} , title:{borrowedBook.Book.Title} , " +
            $"Category: {borrowedBook.Book.Category.Name} , BorrowDate:{borrowedBook.BorrowDate}");
    }
}
