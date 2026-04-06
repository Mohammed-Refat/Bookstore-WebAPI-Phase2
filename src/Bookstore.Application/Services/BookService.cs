using Bookstore.Application.DTOs;
using Bookstore.Domain.Entities;
using Bookstore.Domain.Interfaces;

namespace Bookstore.Application.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _repository;

    public BookService(IBookRepository repository)
    {
        _repository = repository;
    }

    public async Task<BookDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var book = await _repository.GetByIdAsync(id, ct);
        return book is null ? null : MapToDto(book);
    }

    public async Task<IReadOnlyList<BookDto>> GetAllAsync(CancellationToken ct = default)
    {
        var books = await _repository.GetAllAsync(ct);
        return books.Select(MapToDto).ToList();
    }

    public async Task<BookDto> CreateAsync(CreateBookRequest request, CancellationToken ct = default)
    {
        var book = Book.Create(request.Title, request.Author, request.Price, request.StockQuantity);
        await _repository.AddAsync(book, ct);
        return MapToDto(book);
    }

    public async Task<BookDto> UpdateAsync(Guid id, UpdateBookRequest request, CancellationToken ct = default)
    {
        var book = await _repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Book with id '{id}' was not found.");

        book.Update(request.Title, request.Author, request.Price, request.StockQuantity);
        await _repository.UpdateAsync(book, ct);
        return MapToDto(book);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var book = await _repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Book with id '{id}' was not found.");

        await _repository.DeleteAsync(book.Id, ct);
    }

    private static BookDto MapToDto(Book book) => new(
        book.Id,
        book.Title,
        book.Author,
        book.Price,
        book.StockQuantity,
        book.CreatedAt
    );
}