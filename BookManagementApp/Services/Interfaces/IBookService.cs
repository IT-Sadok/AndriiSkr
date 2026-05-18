using BookManagementApp.Models;

namespace BookManagementApp.Services.Interfaces;

public interface IBookService
{
    string GetAllAvailableBooks();
    string GetAllByAuthor(string author);
    string GetByTitle(string title);
    string AddBook(Book book);
    string RemoveByUniqueCode(int uniqueCode);
    string ChangeStatus(string title);
    bool IsTitleExists(string title);
    bool IsUniqueCodeExists(int uniqueCode);
}