using System.Text.Json;
using BookManagementApp.Models;
using BookManagementApp.Models.Enums;
using BookManagementApp.Services.Interfaces;

namespace BookManagementApp;

public class ConsoleMenu(IBookService service)
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };
    
    public void Start()
    {
        while (true)
        {
            Console.WriteLine("\nChoose action:\n");

            foreach (var menuAction in Enum.GetValues<ConsoleMenuAction>())
            {
                Console.WriteLine($"{(int)menuAction} - {menuAction}");
            }
            
            Console.WriteLine();

            var value = Console.ReadLine();

            var isValid = int.TryParse(value, out var action);

            if (!isValid)
            {
                Console.WriteLine("Incorrect input!");
                continue;
            }

            switch ((ConsoleMenuAction)action)
            {
                case ConsoleMenuAction.AddBook:
                    AddBook();
                    break;

                case ConsoleMenuAction.RemoveBook:
                    RemoveBook();
                    break;

                case ConsoleMenuAction.SearchByAuthor:
                    SearchBooksByAuthor();
                    break;

                case ConsoleMenuAction.SearchByTitle:
                    SearchByTitle();
                    break;

                case ConsoleMenuAction.ViewAvailableBooks:
                    GetAllBooks();
                    break;

                case ConsoleMenuAction.ChangeStatus:
                    ChangeStatus();
                    break;

                default:
                    Console.WriteLine("Invalid action");
                    break;
            }
        }
    }

    private void GetAllBooks()
    {
        var books = service.GetAllAvailableBooks();
        
        if (books.Count == 0)
        {
            Console.WriteLine("No available books found");
            return;
        }
        
        var json = JsonSerializer.Serialize(books, _jsonOptions);
        
        Console.WriteLine(json);
    }

    private void SearchBooksByAuthor()
    {
        while (true)
        {
            Console.Write("Type author of the book (or q to return): ");
            var input = Console.ReadLine();

            if (input == "q")
                return;

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Author cannot be empty!");
                continue;
            }
            
            var books = service.GetAllByAuthor(input);

            if (books.Count == 0)
            {
                Console.WriteLine($"No books found for author: {input}");
                continue;
            }

            var json = JsonSerializer.Serialize(books, _jsonOptions);
            
            Console.WriteLine($"\nSearch result:\n{json}");
            return;
        }
    }

    private void SearchByTitle()
    {
        while (true)
        {
            Console.Write("Type title of the book (or q to return): ");
            var input = Console.ReadLine();

            if (input == "q")
                return;

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Title cannot be empty!");
                continue;
            }

            var book = service.GetByTitle(input);

            if (book is null)
            {
                Console.WriteLine($"No book found for title: {input}");
                continue;
            }
            
            var json = JsonSerializer.Serialize(book, _jsonOptions);
            
            Console.WriteLine($"\nSearch result:\n{json}");
            return;
        }
    }

    private void AddBook()
    {
        var book = new Book();

        var isExit = AddTitle(book);
        
        if (!isExit) return;
        
        AddAuthor(book);
        AddReleaseYear(book);
        AddIsbn(book);

        var addedBook = service.AddBook(book);

        if (addedBook is null)
        {
            Console.WriteLine("\nBook with this ISBN already exists!");
            return;
        }

        Console.WriteLine($"\n'{addedBook.Title}' - Added successfully!");
    }

    private void RemoveBook()
    {
        while (true)
        {
            Console.Write("Type isbn unique code (or q to return): ");

            var input = Console.ReadLine();

            if (input == "q")
                return;

            var isValid = int.TryParse(input, out var isbn);

            if (!isValid || isbn <= 0)
            {
                Console.WriteLine("Invalid isbn unique code! Type a valid isbn unique code.");
                continue;
            }

            var removedBook = service.RemoveByIsbn(isbn);

            if (removedBook is null)
            {
                Console.WriteLine("\nNo book found with this ISBN!");
                continue;
            }

            Console.WriteLine($"\n'{removedBook.Title}' - Removed successfully!");
            return;
        }
    }

    private void ChangeStatus()
    {
        while (true)
        {
            Console.Write("To change status type title of the book (or q to return): ");

            var input = Console.ReadLine();

            if (input == "q")
                return;

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Title cannot be empty!");
                continue;
            }
            
            var updatedBook = service.ChangeStatus(input);

            if (updatedBook is null)
            {
                Console.WriteLine($"No book found for title: {input}");
                continue;
            }
            
            var json = JsonSerializer.Serialize(updatedBook, _jsonOptions);
            
            Console.WriteLine($"Book '{updatedBook.Title}' is now {updatedBook.Status}:\n" + json);
            return;
        }
    }

    #region Private methods

    private bool AddTitle(Book book)
    {
        while (true)
        {
            Console.Write("Type title of the book (or q to return): ");
            var input = Console.ReadLine();

            if (input == "q")
                return false;

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Title cannot be empty!");
                continue;
            }

            if (service.IsTitleExists(input))
            {
                Console.WriteLine("Book with this title already exists!");
                continue;
            }

            book.Title = input;
            return true;
        }
    }

    private void AddAuthor(Book book)
    {
        while (true)
        {
            Console.Write("Type author of the book: ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Author cannot be empty!");
                continue;
            }

            book.Author = input;
            return;
        }
    }

    private void AddReleaseYear(Book book)
    {
        while (true)
        {
            Console.Write("Type release year of the book: ");

            var input = Console.ReadLine();

            if (!int.TryParse(input, out var releaseYear) || releaseYear <= 0)
            {
                Console.WriteLine("Incorrect input! Type a valid year.");
                continue;
            }

            book.ReleaseYear = releaseYear;
            return;
        }
    }

    private void AddIsbn(Book book)
    {
        while (true)
        {
            Console.Write("Type isbn unique code of the book: ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("ISBN cannot be empty!");
                continue;
            }

            if (!int.TryParse(input, out var isbn) || isbn <= 0)
            {
                Console.WriteLine("Incorrect input! Type a valid isbn unique code.");
                continue;
            }

            if (service.IsIsbnExists(isbn))
            {
                Console.WriteLine("Book with this isbn unique code already exists!");
                continue;
            }

            book.Isbn = isbn;
            return;
        }
    }

    #endregion
}