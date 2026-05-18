using BookManagementApp.Models;
using BookManagementApp.Repositories.Interfaces;
using BookManagementApp.Data.Interfaces;

namespace BookManagementApp.Repositories;

public class BookRepository(IJsonDataHelper jsonDataHelper) 
    : IBookRepository
{
    public List<Book> GetAllAvailableBooks()
    {
        return jsonDataHelper
            .ReadAll()
            .Where(x => x.Status)
            .ToList();
    }

    public List<Book> GetAllByAuthor(string author)
    {
        return jsonDataHelper
            .ReadAll()
            .Where(x => x.Author == author)
            .ToList();
    }

    public Book? GetByTitle(string title)
    {
        return jsonDataHelper
            .ReadAll()
            .FirstOrDefault(x => x.Title == title);
    }
    
    public bool AddBook(Book book)
    {
        var books = jsonDataHelper.ReadAll();

        var alreadyExists = books.Any(x =>
            x.UniqueCode == book.UniqueCode);

        if (alreadyExists)
            return false;

        books.Add(book);

        jsonDataHelper.WriteAll(books);

        return true;
    }
    
    public bool RemoveByUniqueCode(int uniqueCode)
    {
        var books = jsonDataHelper.ReadAll();

        var book = books
            .FirstOrDefault(x => x.UniqueCode == uniqueCode);

        if (book is null)
            return false;

        books.Remove(book);

        jsonDataHelper.WriteAll(books);

        return true;
    }

    public Book? ChangeStatus(string title)
    {
        var books = jsonDataHelper.ReadAll();

        var book = books.FirstOrDefault(x =>
            x.Title == title);

        if (book is null)
            return null;

        book.Status = !book.Status;

        jsonDataHelper.WriteAll(books);
        return book;
    }
    
    public bool IsTitleExists(string title)
    {
        return jsonDataHelper
            .ReadAll()
            .Any(x => x.Title == title);
    }

    public bool IsUniqueCodeExists(int uniqueCode)
    {
        return jsonDataHelper
            .ReadAll()
            .Any(x => x.UniqueCode == uniqueCode);
    }
}

