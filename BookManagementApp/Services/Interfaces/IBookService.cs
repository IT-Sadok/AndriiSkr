using BookManagementApp.Models;

namespace BookManagementApp.Services.Interfaces;

public interface IBookService
{
    List<Book> GetAllAvailableBooks();
    List<Book> GetAllByAuthor(string author);
    Book? GetByTitle(string title);
    Book? AddBook(Book book);
    Book? RemoveByIsbn(int isbn);
    Book? ChangeStatus(string title);
    Book? UpdateTitle(int isbn, string newTitle);
    Book? UpdateAuthor(int isbn, string newAuthor);
    bool IsTitleExists(string title);
    bool IsIsbnExists(int isbn);
}