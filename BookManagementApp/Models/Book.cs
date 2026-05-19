using BookManagementApp.Models.Enums;

namespace BookManagementApp.Models;

public class Book
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public int Isbn { get; set; }
    public BookStatus Status { get; set; } = BookStatus.Available;
}

