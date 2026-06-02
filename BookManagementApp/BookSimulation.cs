using BookManagementApp.Models;
using BookManagementApp.Models.Enums;

namespace BookManagementApp.Simulation;

public class BookSimulation
{
    private const int TaskCount = 100;
    private readonly List<Book> _books;
    private readonly SemaphoreSlim _semaphore = new(100, 100);
    private readonly Lock _lock = new();
    private readonly Random _random = new();

    public BookSimulation()
    {
        _books = new List<Book>();
        
        for (int i = 1; i <= 20; i++)
        {
            _books.Add(new Book
            {
                Title = $"Book {i}", Author = $"Author {i}", ReleaseYear = 2000 + i, Isbn = 1000 + i,
                Status = BookStatus.Available
            });
        }
    }

    public async Task RunAsync()
    {
        Console.WriteLine($"\n=== Simulation started: {TaskCount} tasks ===\n");
        
        var tasks = new Task[TaskCount];
        
        for (int i = 0; i < TaskCount; i++)
        {
            int taskId = i;
            tasks[taskId] = Task.Run(() => ExecuteTask(taskId));
        }

        await Task.WhenAll(tasks);
        
        Console.WriteLine($"\n=== Simulation finished. Books: ===\n");
        
        foreach (var book in _books)
            Console.WriteLine(
                $" [{book.Status}] ISBN:{book.Isbn} | \"{book.Title}\" by {book.Author} ({book.ReleaseYear})");
    }

    private async Task ExecuteTask(int taskId)
    {
        await _semaphore.WaitAsync();
        
        try
        {
            await Task.Delay(_random.Next(10, 50));
            lock (_lock)
            {
                var book = _books[_random.Next(_books.Count)];
                var action = (SimulationAction)(taskId % 4);
                
                switch (action)
                {
                    case SimulationAction.EditTitle:
                        Console.WriteLine($"[Task {taskId}] EditTitle: '{book.Title}' -> 'Book edited by {taskId}'");
                        book.Title = $"Book edited by {taskId}";
                        break;
                    
                    case SimulationAction.EditAuthor:
                        Console.WriteLine($"[Task {taskId}] EditAuthor: '{book.Author}' -> 'Author_{taskId}'");
                        book.Author = $"Author_{taskId}";
                        break;
                    
                    case SimulationAction.ChangeStatus:
                        var newStatus = book.Status == BookStatus.Available
                            ? BookStatus.Borrowed
                            : BookStatus.Available;
                        Console.WriteLine($"[Task {taskId}] ChangeStatus: '{book.Title}' {book.Status} -> {newStatus}");
                        book.Status = newStatus;
                        break;
                    
                    case SimulationAction.EditYear:
                        var newYear = 1990 + _random.Next(35);
                        Console.WriteLine($"[Task {taskId}] EditYear: '{book.Title}' {book.ReleaseYear} -> {newYear}");
                        book.ReleaseYear = newYear;
                        break;
                }
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }
}