using Microsoft.AspNetCore.Mvc;
using LuxeFemStore.Data;
using LuxeFemStore.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace LuxeFemStore.Controllers;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;

    // This "Constructor" asks the system for the Database bridge we set up in Program.cs
    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult SignUp() => View();

    [HttpPost]
    public IActionResult SignUp(User user)
    {
        if (ModelState.IsValid)
        {
            // 1. Add the user to the list
            _context.Users.Add(user); 
            // 2. Push the change to the actual SQL Database
            _context.SaveChanges();    
            
            return RedirectToAction("SignIn");
        }
        return View(user);
    }

    [HttpGet]
    public IActionResult SignIn() => View();

    [HttpPost]
    public IActionResult SignIn(string email, string password)
    {
        // Search the database for a user with matching email AND password
        var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
        
        if (user != null)
        {
            // If found, store their name in a Session so the website "remembers" them
            HttpContext.Session.SetString("UserName", user.Username);
            return RedirectToAction("Index", "Shop");
        }

        ViewBag.Error = "Invalid email or password. Please try again.";
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("SignUp");
    }
}