using System.Text.Json;
using BookManagementApp.Models;
using BookManagementApp.Data.Interfaces;

namespace BookManagementApp.Data;

public class JsonDataHelper : IJsonDataHelper
{
    private const string JsonPath = "Books.json";

    private readonly SemaphoreSlim _semaphore = new(1, 1);
    
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };

    public List<Book> ReadAll()
    {
        _semaphore.Wait();
        try
        {
            if (!File.Exists(JsonPath))
                return [];

            var json = File.ReadAllText(JsonPath);

            if (string.IsNullOrEmpty(json))
                return [];

            var books = JsonSerializer.Deserialize<List<Book>>(json);

            return books ?? [];
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public void WriteAll(List<Book> books)
    {
        _semaphore.Wait();
        try
        {
            var json = JsonSerializer.Serialize(books, _jsonOptions);
            File.WriteAllText(JsonPath, json);
        }
        finally
        {
            _semaphore.Release();
        }
    }
}