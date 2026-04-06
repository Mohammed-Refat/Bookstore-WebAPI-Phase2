namespace Bookstore.Application.DTOs;

public record BookDto(
    Guid Id,
    string Title,
    string Author,
    decimal Price,
    int StockQuantity,
    DateTime CreatedAt
);