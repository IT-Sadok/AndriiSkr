using System.Text.Json;
using BookManagementApp.Models;
using BookManagementApp.Repositories.Interfaces;
using BookManagementApp.Services.Interfaces;

namespace BookManagementApp.Services;

public class BookService(IBookRepository repository)
    : IBookService
{
    public List<Book> GetAllAvailableBooks()
    {
        return repository.GetAllAvailableBooks();
    }

    public List<Book> GetAllByAuthor(string author)
    {
        return repository.GetAllByAuthor(author);
    }

    public Book? GetByTitle(string title)
    {
        return repository.GetByTitle(title);
    }

    public Book? AddBook(Book book)
    {
        return repository.AddBook(book);
    }

    public Book? RemoveByIsbn(int isbn)
    {
        return repository.RemoveByIsbn(isbn);
    }

    public Book? ChangeStatus(string title)
    {
        return repository.ChangeStatus(title);
    }

    public Book? UpdateTitle(int isbn, string newTitle)
    {
        return repository.UpdateTitle(isbn, newTitle);
    }

    public Book? UpdateAuthor(int isbn, string newAuthor)
    {
        return repository.UpdateAuthor(isbn, newAuthor);
    }

    public bool IsTitleExists(string title)
    {
        return repository.IsTitleExists(title);
    }

    public bool IsIsbnExists(int isbn)
    {
        return repository.IsIsbnExists(isbn);
    }
}