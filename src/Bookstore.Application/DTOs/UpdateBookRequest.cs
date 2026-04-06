namespace Bookstore.Application.DTOs;

public record UpdateBookRequest(
    string Title,
    string Author,
    decimal Price,
    int StockQuantity
);