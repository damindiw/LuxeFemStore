using Microsoft.AspNetCore.Mvc;
using LuxeFemStore.Models;
using Newtonsoft.Json; // Required for List Serialization

namespace LuxeFemStore.Controllers;

public class ShopController : Controller
{
    // Generates a comprehensive list of products with Premium Naming Conventions
    private List<Product> GetMasterProductList()
    {
        var list = new List<Product>();

        // --- 1. HANDBAGS BEAUTY NAMES ---
        var bagNames = new Dictionary<string, string[]> {
            { "Crossbody", new[] { "Stellar", "Luna", "Velvet", "Aria", "Willow" } },
            { "Tote", new[] { "Heritage", "Horizon", "Empire", "Savoy", "Regal" } },
            { "Hobo", new[] { "Nomad", "Serene", "Boho-Luxe", "Essence", "Azure" } },
            { "Weekender", new[] { "Odyssey", "Voyage", "Escape", "Grand", "Latitude" } }
        };

        foreach (var sub in bagNames.Keys) {
            for (int i = 0; i < 5; i++) {
                var p = new Product {
                    Id = list.Count + 1,
                    Name = $"{bagNames[sub][i]} {sub}",
                    Category = "Handbags",
                    SubCategory = sub,
                    Price = 4500 + ((i + 1) * 500),
                    ImageUrl = $"/images/{sub.ToLower()}{i + 1}.jpg",
                    Description = $"A masterpiece of design, this {sub} bag defines modern elegance.",
                    StockQuantity = 10
                };
                p.ProductReviews.Add(new Review { Name = "Ishani W.", Comment = "Absolutely stunning!", Rating = 5 });
                list.Add(p);
            }
        }

        // --- 2. MAKEUP BEAUTY NAMES ---
        var makeupNames = new Dictionary<string, string[]> {
            { "Lipsticks", new[] { "Rose Petal", "Crimson Vow", "Mauve Silk", "Ruby Envy", "Nude Aura" } },
            { "Foundations", new[] { "Flawless", "Second Skin", "Radiant", "HD Matte", "Velvet" } },
            { "Blushes", new[] { "Morning Dew", "Sunset Glow", "Peony Flush", "Coral Bloom", "Soft Iris" } },
            { "Eyeshadows", new[] { "Celestial", "Earthly", "Midnight", "Ethereal", "Twilight" } },
            { "Eyeliners", new[] { "Precision", "Ink Noir", "Infinity", "Vivid", "Stark" } }
        };

        foreach (var sub in makeupNames.Keys) {
            for (int i = 0; i < 5; i++) {
                var p = new Product {
                    Id = list.Count + 1,
                    Name = $"{makeupNames[sub][i]} {sub}",
                    Category = "Makeup",
                    SubCategory = sub,
                    Price = 1200 + ((i + 1) * 200),
                    ImageUrl = $"/images/{sub.ToLower()}{i + 1}.jpg",
                    Description = $"Enhance your natural beauty with our signature {sub} collection.",
                    StockQuantity = 15
                };
                p.ProductReviews.Add(new Review { Name = "Kavindi P.", Comment = "Best quality ever!", Rating = 5 });
                list.Add(p);
            }
        }

        // --- 3. CLOTHING BEAUTY NAMES ---
        var clothingNames = new Dictionary<string, string[]> {
            { "Casual", new[] { "Breeze", "Urban", "Sunday", "Linen", "Essential" } },
            { "Sport", new[] { "Velocity", "Zenith", "Endurance", "Active", "Peak" } },
            { "Formal", new[] { "Executive", "Majesty", "Grace", "Signature", "Elite" } },
            { "Elegant", new[] { "Gala", "Soirée", "Enchanted", "Silk", "Ritz" } }
        };

        foreach (var sub in clothingNames.Keys) {
            for (int i = 0; i < 5; i++) {
                var p = new Product {
                    Id = list.Count + 1,
                    Name = $"{clothingNames[sub][i]} {sub}",
                    Category = "Clothing",
                    SubCategory = sub,
                    Price = 2500 + ((i + 1) * 400),
                    ImageUrl = $"/images/{sub.ToLower()}{i + 1}.jpg",
                    Description = $"Crafted for comfort and style, our {sub} wear is a wardrobe essential.",
                    StockQuantity = 8
                };
                p.ProductReviews.Add(new Review { Name = "Dilini R.", Comment = "Fits like a dream.", Rating = 4 });
                list.Add(p);
            }
        }

        return list;
    }

    public IActionResult Index()
    {
        var products = GetMasterProductList().Take(3).ToList();
        return View(products);
    }

    [HttpPost]
    public IActionResult AddToCart(int productId)
    {
        var cartJson = HttpContext.Session.GetString("CartItems");
        List<int> cartIds;

        if (string.IsNullOrEmpty(cartJson))
        {
            cartIds = new List<int>();
        }
        else
        {
            cartIds = JsonConvert.DeserializeObject<List<int>>(cartJson);
        }

        cartIds.Add(productId);
        HttpContext.Session.SetString("CartItems", JsonConvert.SerializeObject(cartIds));

        return RedirectToAction("Cart");
    }

    public IActionResult Cart()
    {
        var cartJson = HttpContext.Session.GetString("CartItems");
        if (string.IsNullOrEmpty(cartJson))
        {
            return View(new List<Product>());
        }

        var cartIds = JsonConvert.DeserializeObject<List<int>>(cartJson);
        var allProducts = GetMasterProductList();

        // This maps the IDs in the session back to the full Product objects
        var cartProducts = allProducts.Where(p => cartIds.Contains(p.Id)).ToList();

        return View(cartProducts);
    }

    public IActionResult Category(string name, string sub, string sortOrder)
    {
        var products = GetMasterProductList().Where(p => p.Category == name).AsQueryable();

        if (!string.IsNullOrEmpty(sub))
        {
            products = products.Where(p => p.SubCategory == sub);
        }

        switch (sortOrder)
        {
            case "price_low":
                products = products.OrderBy(p => p.Price);
                break;
            case "price_high":
                products = products.OrderByDescending(p => p.Price);
                break;
            default:
                products = products.OrderBy(p => p.Name);
                break;
        }

        ViewBag.CategoryName = name;
        ViewBag.CurrentSubCategory = sub;
        
        return View(products.ToList());
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
                        p.Category.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                        p.SubCategory.Contains(query, StringComparison.OrdinalIgnoreCase))
            .ToList();

        ViewBag.SearchQuery = query;
        return View("Category", results);
    }

    [HttpPost]
public IActionResult RemoveFromCart(int productId)
{
    var cartJson = HttpContext.Session.GetString("CartItems");
    if (!string.IsNullOrEmpty(cartJson))
    {
        var cartIds = JsonConvert.DeserializeObject<List<int>>(cartJson);
        
        // Remove the first instance of this product ID
        cartIds.Remove(productId);

        // Save the updated list back to session
        HttpContext.Session.SetString("CartItems", JsonConvert.SerializeObject(cartIds));
    }
    return RedirectToAction("Cart");
}

public IActionResult Checkout()
{
    var cartJson = HttpContext.Session.GetString("CartItems");
    if (string.IsNullOrEmpty(cartJson)) return RedirectToAction("Index");

    var cartIds = JsonConvert.DeserializeObject<List<int>>(cartJson);
    var allProducts = GetMasterProductList();
    var cartProducts = allProducts.Where(p => cartIds.Contains(p.Id)).ToList();

    return View(cartProducts); // We'll send the list to a "Confirm Order" page
}

[HttpPost]
public IActionResult PlaceOrder()
{
    // Clear the cart after the order is "placed"
    HttpContext.Session.Remove("CartItems");
    
    return View("OrderSuccess");
}

}