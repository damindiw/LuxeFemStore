namespace LuxeFemStore.Models;

public class Review
{
    public string Name { get; set; }
    public string Comment { get; set; }
    public int Rating { get; set; } // 1 to 5 stars
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // e.g., "Handbags" or "Makeup"
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? SubCategory { get; set; }

    // Add this new property
    public int StockQuantity { get; set; }
    public List<Review> ProductReviews { get; set; } = new List<Review>();
}