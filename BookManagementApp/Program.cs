using BookManagementApp;
using BookManagementApp.Data;
using BookManagementApp.Repositories;
using BookManagementApp.Repositories.Interfaces;
using BookManagementApp.Services;
using BookManagementApp.Services.Interfaces;

var jsonDataHelper = new JsonDataHelper();

IBookRepository repository = new BookRepository(jsonDataHelper);

IBookService service = new BookService(repository);

var menu = new ConsoleMenu(service);

menu.Start();