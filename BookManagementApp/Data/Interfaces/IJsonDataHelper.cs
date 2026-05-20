using BookManagementApp.Models;

namespace BookManagementApp.Data.Interfaces;

public interface IJsonDataHelper
{
    List<Book> ReadAll();

    void WriteAll(List<Book> books);
}