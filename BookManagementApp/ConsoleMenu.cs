using BookManagementApp.Models;
using BookManagementApp.Models.Enums;
using BookManagementApp.Services.Interfaces;

namespace BookManagementApp;

public class ConsoleMenu(IBookService service)
{
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
        var result = service.GetAllAvailableBooks();
        Console.WriteLine(result);
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

            var result = service.GetAllByAuthor(input);

            Console.WriteLine($"\nSearch result:\n{result}");
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

            var result = service.GetByTitle(input);

            Console.WriteLine($"\nSearch result:\n{result}");
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
        AddUniqueCode(book);

        var result = service.AddBook(book);
        Console.WriteLine("\n" + result);
    }

    private void RemoveBook()
    {
        while (true)
        {
            Console.Write("Type unique code (or q to return): ");

            var input = Console.ReadLine();

            if (input == "q")
                return;

            var isValid = int.TryParse(input, out var uniqueCode);

            if (!isValid || uniqueCode <= 0)
            {
                Console.WriteLine("Invalid unique code! Type a valid unique code.");
                continue;
            }

            var result = service.RemoveByUniqueCode(uniqueCode);

            Console.WriteLine("\n" + result);
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

            var result = service.ChangeStatus(input);

            Console.WriteLine(result);
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

    private void AddUniqueCode(Book book)
    {
        while (true)
        {
            Console.Write("Type unique code of the book: ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Unique Code cannot be empty!");
                continue;
            }

            if (!int.TryParse(input, out var uniqueCode) || uniqueCode <= 0)
            {
                Console.WriteLine("Incorrect input! Type a valid Unique Code.");
                continue;
            }

            if (service.IsUniqueCodeExists(uniqueCode))
            {
                Console.WriteLine("Book with this unique code already exists!");
                continue;
            }

            book.UniqueCode = uniqueCode;
            return;
        }
    }

    #endregion
}