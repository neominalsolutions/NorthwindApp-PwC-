using Microsoft.AspNetCore.Mvc;
using NorthwindApp.Models;
using System.Diagnostics;

namespace NorthwindApp.Controllers
{
  // ana giriş kurallarını yönettiğimiz denetleyici
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
      _logger = logger;
    }

    // bu tasarım deseni mimarisinde controller içerisine birden fazla sayfa açabiliriz.
    public IActionResult Index()
    {
      return View();
    }

    // her bir action bir sayfaya denk gelir.
    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}