using BookManagementApp.Models;

namespace BookManagementApp.Repositories.Interfaces;

public interface IBookRepository
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