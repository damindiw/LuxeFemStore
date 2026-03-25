using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LuxeFemStore.Models;

namespace LuxeFemStore.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserName")))
    {
        return RedirectToAction("SignUp", "Account");
    }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public IActionResult About()
{
    return View();
}
// Action to show the Help Center
public IActionResult Help()
{
    return View();
}

// Action to show the Contact Us page
public IActionResult Contact()
{
    return View();
}
public IActionResult Terms()
{
    return View();
}
}
