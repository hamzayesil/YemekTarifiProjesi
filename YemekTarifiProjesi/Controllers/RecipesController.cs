using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YemekTarifiProjesi.Models;

namespace YemekTarifiProjesi.Controllers

{
    public class RecipesController : Controller
    {
      
            private readonly AppDbContext _context; // Bu satır eksikse ekle

            public RecipesController(AppDbContext context)
            {
                _context = context;
            }
            // ... Buradan sonra Edit metotları gelir ...
        
        // Sayfayı Açan Metot (GET)
      
        private static List<Recipe> _recipes = new List<Recipe>();


        // GET: Recipes
        // GET: Recipes
        public async Task<IActionResult> Index(string searchString)
        {
            // Veritabanı sorgusunu hazırla
            var recipes = from r in _context.Recipes
                          select r;

            // Arama kutusu doluysa filtrele
            if (!String.IsNullOrEmpty(searchString))
            {
                // Description (Açıklama) olmadığı için sadece Name (İsim) içinde arıyoruz
                recipes = recipes.Where(s => s.Name.Contains(searchString));
            }

            return View(await recipes.ToListAsync());
        }


        public IActionResult Create()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Recipe yemek) // Parametre adı sende farklı olabilir
        {
            try
            {
                // Senin mevcut kaydetme kodların burada kalsın
                _context.Add(yemek);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception hata)
            {
                // HATA VARSA BURASI ÇALIŞACAK VE EKRANA YAZACAK
                var mesaj = hata.InnerException != null ? hata.InnerException.Message : hata.Message;
                return Content("HATA DETAYI: " + mesaj);
            }
        }
        // Silme işlemi için Controller kodu
        [HttpPost]
        public IActionResult Sil(int id)
        {
            var tarif = _context.Recipes.Find(id); // 'Recipes' senin tablonun adı olmalı
            if (tarif != null)
            {
                _context.Recipes.Remove(tarif);
                _context.SaveChanges();
            }
            return RedirectToAction("Index"); // Silince listeye geri dön
        }

        public IActionResult Delete(int id)
        {
            var recipe = _recipes.FirstOrDefault(r => r.Id == id);
            if (recipe != null) _recipes.Remove(recipe);
            return RedirectToAction(nameof(Index));
        }
    }
}