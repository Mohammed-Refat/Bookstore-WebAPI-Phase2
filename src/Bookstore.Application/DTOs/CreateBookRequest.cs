namespace Bookstore.Application.DTOs;

public record CreateBookRequest(
    string Title,
    string Author,
    decimal Price,
    int StockQuantity
);