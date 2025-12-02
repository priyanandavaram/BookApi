using BookApi.Models;
using System.Collections.Concurrent;

namespace BookApi.Services;

public class BookService
{
    // Use a thread-safe collection so it works fine under concurrent requests when run locally.
    private readonly ConcurrentDictionary<int, Book> _books = new();
    private int _nextId = 1;

    public void Add(Book book)
    {
        var id = System.Threading.Interlocked.Increment(ref _nextId);
        book.Id = id;
        _books[id] = book;
    }

    public IEnumerable<Book> GetAll() => _books.Values.OrderBy(b => b.Id);

    public bool Checkout(int id)
    {
        if (!_books.TryGetValue(id, out var book)) return false;
        if (book.IsCheckedOut) return false;
        book.IsCheckedOut = true;
        return true;
    }

    public bool Return(int id)
    {
        if (!_books.TryGetValue(id, out var book)) return false;
        if (!book.IsCheckedOut) return false;
        book.IsCheckedOut = false;
        return true;
    }
}
