namespace BookApi.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCheckedOut { get; set; }
}
