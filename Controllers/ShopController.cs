using Microsoft.AspNetCore.Mvc;
using LuxeFemStore.Models;

namespace LuxeFemStore.Controllers;

public class ShopController : Controller
{
    // A single master list so we don't have to keep repeating it
    private List<Product> GetMasterProductList()
    {
        return new List<Product>
        {
            // --- Handbags ---
            new Product { Id = 1, Name = "Leather Handbag", Category = "Handbags", Price = 120.00m, ImageUrl = "/images/bag1.jpg", Description = "Premium quality leather handbag with gold-tone hardware.",StockQuantity = 12 },
            new Product { Id = 4, Name = "Green Bucket Bag", Category = "Handbags", Price = 95.00m, ImageUrl = "/images/bag2.jpg", Description = "Stylish green bucket bag perfect for daily use.",StockQuantity = 0 },
            new Product { Id = 5, Name = "Black Padlock Bag", Category = "Handbags", Price = 110.00m, ImageUrl = "/images/bag3.jpg", Description="Amazing black padlock bag for handbag's lovers.",StockQuantity = 2 },
            
            // --- Makeup ---
            new Product { Id = 2, Name = "Matte Lipstick", Category = "Makeup", Price = 25.00m, ImageUrl = "/images/makeup1.jpg", Description = "Long-lasting matte finish for a bold look.",StockQuantity = 10 },
            new Product { Id = 6, Name = "Foundation Cream", Category = "Makeup", Price = 45.00m, ImageUrl = "/images/makeup2.jpg", Description="A smooth base makeup that evens out skin tone.",StockQuantity = 2 },
            new Product { Id = 7, Name = "Eyeshadow Palette", Category = "Makeup", Price = 60.00m, ImageUrl = "/images/makeup3.jpg", Description="A collection of colors used to enhance and define the eyes.",StockQuantity = 0 },
            
            // --- Clothing ---
            new Product { Id = 3, Name = "Silk Saree Material", Category = "Clothing", Price = 80.00m, ImageUrl = "/images/clothing1.jpg", Description = "Soft silk material with traditional patterns." ,StockQuantity = 22},
            new Product { Id = 8, Name = "Cotton Dress Fabric", Category = "Clothing", Price = 40.00m, ImageUrl = "/images/clothing2.jpg", Description="A soft, breathable material ideal for comfortable everyday wear.",StockQuantity = 0 },
            new Product { Id = 9, Name = "Linen Suit Material", Category = "Clothing", Price = 110.00m, ImageUrl = "/images/clothing3.jpg", Description="A lightweight, durable fabric known for its cool and elegant finish." ,StockQuantity = 10}
        };
    }

    public IActionResult Index()
    {
        var products = GetMasterProductList().Take(3).ToList();
        return View(products);
    }

    [HttpPost]
    public IActionResult AddToCart(int productId)
    {
        HttpContext.Session.SetInt32("LastAddedItem", productId);
        return RedirectToAction("Cart");
    }

    public IActionResult Cart()
    {
        int? itemId = HttpContext.Session.GetInt32("LastAddedItem");
        if (itemId == null) return View(null);

        var product = GetMasterProductList().FirstOrDefault(p => p.Id == itemId);
        return View(product);
    }

    // ONLY ONE Category function (This fixes the AmbiguousMatchException)
    public IActionResult Category(string name, string sortOrder)
    {
        var allProducts = GetMasterProductList();
        var filteredProducts = allProducts.Where(p => p.Category == name).ToList();

        // Apply Sorting
        switch (sortOrder)
        {
            case "price_low":
                filteredProducts = filteredProducts.OrderBy(p => p.Price).ToList();
                break;
            case "price_high":
                filteredProducts = filteredProducts.OrderByDescending(p => p.Price).ToList();
                break;
        }

        ViewBag.CategoryName = name;
        ViewBag.CurrentSort = sortOrder;
        return View(filteredProducts);
    }

    public IActionResult Details(int id)
    {
        var allProducts = GetMasterProductList();
        var product = allProducts.FirstOrDefault(p => p.Id == id);

        if (product == null) return NotFound();

        var related = allProducts
            .Where(p => p.Category == product.Category && p.Id != id)
            .Take(3)
            .ToList();

        ViewBag.RelatedProducts = related;
        return View(product);
    }

    public IActionResult Search(string query)
    {
        if (string.IsNullOrEmpty(query)) return RedirectToAction("Index");

        var results = GetMasterProductList()
            .Where(p => p.Name.Contains(query, StringComparison.OrdinalIgnoreCase) || 
                        p.Category.Contains(query, StringComparison.OrdinalIgnoreCase))
            .ToList();

        ViewBag.SearchQuery = query;
        return View("Category", results);
    }
    
}