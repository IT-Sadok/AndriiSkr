using BookManagementApp.Models;
using BookManagementApp.Models.Enums;
using BookManagementApp.Services.Interfaces;

namespace BookManagementApp;

public class BookSimulation(IBookService bookService)
{
    private const int TaskCount = 100;

    private readonly Random _random = new();

    public async Task RunAsync()
    {
        var books = bookService.GetAllAvailableBooks();

        if (books.Count == 0)
        {
            Console.WriteLine("No books found. Add some books first.");
            return;
        }

        Console.WriteLine($"\n=== Simulation started: {TaskCount} tasks, {books.Count} books loaded ===\n");

        var tasks = new Task[TaskCount];

        for (int i = 0; i < TaskCount; i++)
        {
            int taskId = i;
            tasks[taskId] = Task.Run(() => ExecuteTask(taskId, books));
        }

        await Task.WhenAll(tasks);

        Console.WriteLine($"\n=== Simulation finished ===\n");
    }

    private async Task ExecuteTask(int taskId, List<Book> books)
    {
        await Task.Delay(_random.Next(10, 50));

        var book = books[_random.Next(books.Count)];
        var action = (SimulationAction)(taskId % 4);

        switch (action)
        {
            case SimulationAction.EditTitle:
                Console.WriteLine($"[Task {taskId}] EditTitle on '{book.Title}'");
                bookService.UpdateTitle(book.Isbn, $"Book edited by {taskId}");
                break;

            case SimulationAction.EditAuthor:
                Console.WriteLine($"[Task {taskId}] EditAuthor on '{book.Title}'");
                bookService.UpdateAuthor(book.Isbn, $"Author_{taskId}");
                break;

            case SimulationAction.ChangeStatus:
                Console.WriteLine($"[Task {taskId}] ChangeStatus on '{book.Title}'");
                bookService.ChangeStatus(book.Title);
                break;

            case SimulationAction.EditYear:
                Console.WriteLine($"[Task {taskId}] GetByTitle '{book.Title}'");
                bookService.GetByTitle(book.Title);
                break;
        }
    }
}
