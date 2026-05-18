using BookManagementApp.Models;

namespace BookManagementApp.Repositories.Interfaces;

public interface IBookRepository
{
    List<Book> GetAllAvailableBooks();
    List<Book> GetAllByAuthor(string author);
    Book? GetByTitle(string title);
    bool AddBook(Book book);
    bool RemoveByUniqueCode(int uniqueCode);
    Book? ChangeStatus(string title);
    bool IsTitleExists(string title);
    bool IsUniqueCodeExists(int uniqueCode);
}