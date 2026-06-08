using BookManagementApp.Models;
using BookManagementApp.Repositories.Interfaces;
using BookManagementApp.Data.Interfaces;
using BookManagementApp.Models.Enums;

namespace BookManagementApp.Repositories;

public class BookRepository(IJsonDataHelper jsonDataHelper) : IBookRepository
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public List<Book> GetAllAvailableBooks()
    {
        return jsonDataHelper.ReadAll().Where(x => x.Status == BookStatus.Available).ToList();
    }

    public List<Book> GetAllByAuthor(string author)
    {
        return jsonDataHelper.ReadAll().Where(x => x.Author == author).ToList();
    }

    public Book? GetByTitle(string title)
    {
        return jsonDataHelper.ReadAll().FirstOrDefault(x => x.Title == title);
    }

    public Book? AddBook(Book book)
    {
        _semaphore.Wait();
        try
        {
            var books = jsonDataHelper.ReadAll();
            
            var alreadyExists = books.Any(x => x.Isbn == book.Isbn);
            
            if (alreadyExists) return null;
            
            books.Add(book);

            jsonDataHelper.WriteAll(books);
            
            return book;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public Book? RemoveByIsbn(int isbn)
    {
        _semaphore.Wait();
        try
        {
            var books = jsonDataHelper.ReadAll();
            
            var book = books.FirstOrDefault(x => x.Isbn == isbn);
            
            if (book is null) return null;
            
            books.Remove(book);
            
            jsonDataHelper.WriteAll(books);
            
            return book;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public Book? ChangeStatus(string title)
    {
        _semaphore.Wait();
        try
        {
            var books = jsonDataHelper.ReadAll();
            
            var book = books.FirstOrDefault(x => x.Title == title);
            
            if (book is null) return null;
            
            book.Status = book.Status == BookStatus.Available ? BookStatus.Borrowed : BookStatus.Available;
            
            jsonDataHelper.WriteAll(books);
            
            return book;
        }
        finally
        {
            _semaphore.Release();
        }
    }
    
    public Book? UpdateTitle(int isbn, string newTitle)
    {
        _semaphore.Wait();
        try
        {
            var books = jsonDataHelper.ReadAll();
            
            var book = books.FirstOrDefault(x => x.Isbn == isbn);
            
            if (book is null) return null;
            
            book.Title = newTitle;
            
            jsonDataHelper.WriteAll(books);
            
            return book;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public Book? UpdateAuthor(int isbn, string newAuthor)
    {
        _semaphore.Wait();
        try
        {
            var books = jsonDataHelper.ReadAll();
            
            var book = books.FirstOrDefault(x => x.Isbn == isbn);
            
            if (book is null) return null;
            
            book.Author = newAuthor;
            
            jsonDataHelper.WriteAll(books);
            
            return book;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public bool IsTitleExists(string title)
    {
        return jsonDataHelper.ReadAll().Any(x => x.Title == title);
    }

    public bool IsIsbnExists(int isbn)
    {
        return jsonDataHelper.ReadAll().Any(x => x.Isbn == isbn);
    }
}