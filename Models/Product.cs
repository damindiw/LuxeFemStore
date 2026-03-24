namespace LuxeFemStore.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // e.g., "Handbags" or "Makeup"
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Add this new property
    public int StockQuantity { get; set; }
}