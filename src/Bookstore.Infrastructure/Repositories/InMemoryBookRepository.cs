using Bookstore.Domain.Entities;
using Bookstore.Domain.Interfaces;

namespace Bookstore.Infrastructure.Repositories;

// This is the "chef" — the concrete implementation the DI container will supply
public class InMemoryBookRepository : IBookRepository
{
    // A dictionary acting as our in-memory database
    // Key = Book's Id, Value = the Book itself
    private readonly Dictionary<Guid, Book> _store = new();

    public Task<Book?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        // TryGetValue returns false if not found — we return null in that case
        _store.TryGetValue(id, out var book);
        return Task.FromResult(book);
    }

    public Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken ct = default)
    {
        // Cast the dictionary values to IReadOnlyList and wrap in a completed Task
        IReadOnlyList<Book> books = _store.Values.ToList();
        return Task.FromResult(books);
    }

    public Task AddAsync(Book book, CancellationToken ct = default)
    {
        // Store the book using its Id as the key
        _store[book.Id] = book;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Book book, CancellationToken ct = default)
    {
        // Overwrite the existing entry — same key, new value
        _store[book.Id] = book;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        // Remove by key — does nothing if key doesn't exist
        _store.Remove(id);
        return Task.CompletedTask;
    }
}