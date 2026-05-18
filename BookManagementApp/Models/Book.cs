namespace BookManagementApp.Models;

public class Book
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public int UniqueCode { get; set; }
    public bool Status { get; set; } = true;
}

