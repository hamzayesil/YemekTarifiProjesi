using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Bunu ekledik (Veritabaný iþlemleri için)
using YemekTarifiProjesi.Models;

namespace YemekTarifiProjesi.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context; // 1. Veritabaný baðlantýsýný buraya tanýmladýk

    // 2. Constructor'a Context'i de ekledik (Enjekte ettik)
    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    // 3. Ýþte sihri yapan yer burasý!
    public IActionResult Index()
    {
        // Veritabanýndaki "Recipes" (Tarifler) tablosundan tüm verileri getir ve listeye çevir
        var yemekler = _context.Recipes.ToList();

        // Bu listeyi sayfaya gönder
        return View(yemekler);
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
}