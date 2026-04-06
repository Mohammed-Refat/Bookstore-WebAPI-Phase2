using Bookstore.Application.DTOs;

namespace Bookstore.Application.Services;

public interface IBookService
{
    Task<BookDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<BookDto>> GetAllAsync(CancellationToken ct = default); 
    Task<BookDto> CreateAsync(CreateBookRequest request, CancellationToken ct = default);
    Task<BookDto> UpdateAsync(Guid id, UpdateBookRequest request, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}