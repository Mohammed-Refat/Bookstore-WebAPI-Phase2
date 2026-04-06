namespace Bookstore.Domain.Entities;

public class Book
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // EF Core needs this
    private Book()
    {
        Title = null!;
        Author = null!;
    } 
    public static Book Create(string title, string author, decimal price, int stockQuantity)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required.");
        if (string.IsNullOrWhiteSpace(author)) throw new ArgumentException("Author is required.");
        if (price <= 0) throw new ArgumentException("Price must be positive.");
        if (stockQuantity < 0) throw new ArgumentException("Stock cannot be negative.");

        return new Book
        {
            Id = Guid.NewGuid(),
            Title = title,
            Author = author,
            Price = price,
            StockQuantity = stockQuantity,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string title, string author, decimal price, int stockQuantity)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required.");
        if (string.IsNullOrWhiteSpace(author)) throw new ArgumentException("Author is required.");
        if (price <= 0) throw new ArgumentException("Price must be positive.");
        if (stockQuantity < 0) throw new ArgumentException("Stock cannot be negative.");

        Title = title;
        Author = author;
        Price = price;
        StockQuantity = stockQuantity;
    }
}