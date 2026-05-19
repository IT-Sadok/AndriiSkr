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
    bool IsTitleExists(string title);
    bool IsUniqueCodeExists(int isbn);
}