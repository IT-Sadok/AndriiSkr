using System.Text.Json;
using BookManagementApp.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

const string jsonPath = "Books.json";

var jsonOptions = new JsonSerializerOptions
{
    WriteIndented = true
};

while (true)
{
    Console.WriteLine("\nChoose action:\n");

    Console.WriteLine("1 - Add Book");
    Console.WriteLine("2 - Remove Book");
    Console.WriteLine("3 - Search By Author");
    Console.WriteLine("4 - Search By Title");
    Console.WriteLine("5 - View All Available Books");
    Console.WriteLine("6 - Change Status");
    Console.WriteLine();

    var value = Console.ReadLine();
    var isValid = int.TryParse(value, out var action);

    if (!isValid)
    {
        Console.WriteLine("Incorrect input!");
        continue;
    }

    Console.WriteLine();

    switch (action)
    {
        case 1:
            AddBook();
            break;
        case 2:
            RemoveByUniqueCode();
            break;
        case 3:
            GetByAuthor();
            break;
        case 4:
            GetByTitle();
            break;
        case 5:
            GetAllBooks();
            break;
        case 6:
            ChangeStatus();
            break;
        default:
            Console.WriteLine("Invalid action");
            break;
    }


    void GetAllBooks()
    {
        var books = ReadAll();

        var availableBooks = books.Where(x => x.Status).ToList();

        if (availableBooks.Count == 0)
        {
            Console.WriteLine("No available books found");
            return;
        }

        var booksJson = JsonSerializer.Serialize(availableBooks, jsonOptions);
        Console.WriteLine(booksJson);
    }

    void GetByAuthor()
    {
        var books = ReadAll();

        if (books.Count == 0)
        {
            Console.WriteLine("No books yet");
            return;
        }

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

            var booksByAuthor = books.Where(x => x.Author == input).ToList();

            if (booksByAuthor.Count == 0)
            {
                Console.WriteLine("No books found for this author!");
                continue;
            }

            var bookJson = JsonSerializer.Serialize(booksByAuthor, jsonOptions);
            Console.WriteLine($"\nSearch result:\n{bookJson}");
            return;
        }
    }

    void GetByTitle()
    {
        var books = ReadAll();

        if (books.Count == 0)
        {
            Console.WriteLine("No books yet");
            return;
        }

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

            var bookByTitle = books.FirstOrDefault(x => x.Title == input);

            if (bookByTitle is null)
            {
                Console.WriteLine("No book found for the title: " + input);
                continue;
            }

            var bookJson = JsonSerializer.Serialize<Book>(bookByTitle, jsonOptions);
            Console.WriteLine($"\nSearch result:\n{bookJson}");
            return;
        }
    }

    bool AddTitle(List<Book> books, Book book)
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

            if (books.Any(x => x.Title == input))
            {
                Console.WriteLine("Book with this title already exists!");
                continue;
            }

            book.Title = input;

            return true;
        }
    }

    void AddAuthor(Book book)
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

    void AddReleaseYear(Book book)
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

    void AddUniqueCode(List<Book> books, Book book)
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

            if (books.Any(x => x.UniqueCode == uniqueCode))
            {
                Console.WriteLine("Book with this unique code already exists!");
                continue;
            }

            book.UniqueCode = uniqueCode;
            return;
        }
    }

    void AddBook()
    {
        var books = ReadAll();
        var book = new Book();

        var success = AddTitle(books, book);
        if (!success)
            return;

        AddAuthor(book);
        AddReleaseYear(book);
        AddUniqueCode(books, book);

        books.Add(book);
        WriteAll(books);

        Console.WriteLine($"\n{book.Title} - Added successfully!");
    }

    void RemoveByUniqueCode()
    {
        var books = ReadAll();

        while (true)
        {
            Console.Write("Type unique code of the book (or q to return): ");
            var input = Console.ReadLine();

            if (input == "q")
                return;

            if (!int.TryParse(input, out var uniqueCode) || uniqueCode <= 0)
            {
                Console.WriteLine("Incorrect input! Type a valid Unique Code.\n");
                continue;
            }

            var bookToRemove = books.FirstOrDefault(x => x.UniqueCode == uniqueCode);

            if (bookToRemove is null)
            {
                Console.WriteLine("No book found with this unique code!\n");
                continue;
            }

            books.Remove(bookToRemove);

            WriteAll(books);

            Console.WriteLine($"{bookToRemove.Title} - Removed successfully!");
            return;
        }
    }

    void ChangeStatus()
    {
        var books = ReadAll();

        while (true)
        {
            Console.Write("Type the title of the book to view and change it's status (or q to return): ");
            var input = Console.ReadLine();

            if (input == "q")
                return;

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Title cannot be empty!");
                continue;
            }

            var book = books.FirstOrDefault(x => x.Title == input);

            if (book is null)
            {
                Console.WriteLine($"No book found for the title: '{input}'");
                continue;
            }

            Console.WriteLine($"Current status: {(book.Status ? "Available" : "Unavailable")}");

            while (true)
            {
                Console.Write("To change status type y/n: ");

                var answer = Console.ReadLine();

                if (answer == "n")
                    return;

                if (answer != "y")
                {
                    Console.WriteLine("Incorrect input.");
                    continue;
                }

                book.Status = !book.Status;

                WriteAll(books);

                Console.WriteLine(
                    $"Status updated successfully! New status: {(book.Status ? "Available" : "Unavailable")}");

                return;
            }
        }
    }

//private

    List<Book> ReadAll()
    {
        if (!File.Exists(jsonPath))
            return [];

        var json = File.ReadAllText(jsonPath);

        if (string.IsNullOrEmpty(json))
            return [];

        var books = JsonSerializer.Deserialize<List<Book>>(json);

        return books ?? [];
    }

    void WriteAll(List<Book> books)
    {
        var json = JsonSerializer.Serialize(books, jsonOptions);
        File.WriteAllText(jsonPath, json);
    }
}