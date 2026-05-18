using System.Text.Json;
using BookManagementApp.Models;
using BookManagementApp.Repositories.Interfaces;
using BookManagementApp.Services.Interfaces;

namespace BookManagementApp.Services;

public class BookService(IBookRepository repository)
    : IBookService
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };

    public string GetAllAvailableBooks()
    {
        var books = repository.GetAllAvailableBooks();

        return books.Count == 0 
            ? "No available books found" 
            : JsonSerializer.Serialize(books, _jsonOptions);
    }

    public string GetAllByAuthor(string author)
    {
        var booksByAuthor = repository.GetAllByAuthor(author);
        
        return booksByAuthor.Count == 0 
            ? $"No books found for this author: {author}" 
            : JsonSerializer.Serialize(booksByAuthor, _jsonOptions);
    }

    public string GetByTitle(string title)
    {
        var book = repository.GetByTitle(title);

        return book is null 
            ? $"No book found for title: {title}" 
            : JsonSerializer.Serialize(book, _jsonOptions);
    }

    public string AddBook(Book book)
    {
        var result = repository.AddBook(book);

        return result
            ? $"{book.Title} - Added successfully!"
            : "Book with this unique code already exists!";
    }

    public string RemoveByUniqueCode(int uniqueCode)
    {
        var result = repository.RemoveByUniqueCode(uniqueCode);

        return result
            ? "Book removed successfully!"
            : "No book found with this unique code!";
    }

    public string ChangeStatus(string title)
    {
        var updatedBook = repository.ChangeStatus(title);

        if (updatedBook  is null)
            return $"No book found for title: {title}";
        
        var newStatus = updatedBook.Status
            ? "Available"
            : "Unavailable";

        return $"Book '{updatedBook.Title}' is now {newStatus}";
    }
    
    public bool IsTitleExists(string title)
    {
        return repository.IsTitleExists(title);
    }

    public bool IsUniqueCodeExists(int uniqueCode)
    {
        return repository.IsUniqueCodeExists(uniqueCode);
    }
}