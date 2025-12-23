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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var recipe = await _context.Recipes.FindAsync(id); // Veritabanından tarif çekiliyor
            if (recipe == null) return NotFound();

            return View(recipe);
        }

        // Güncellemeyi Kaydeden Metot (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Recipe recipe)
        {
            if (id != recipe.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(recipe);
                await _context.SaveChangesAsync(); // Veritabanına kaydediliyor
                return RedirectToAction(nameof(Index));
            }
            return View(recipe);
        }
        private static List<Recipe> _recipes = new List<Recipe>();

      
        public IActionResult Index()
        {
            return View(_recipes);
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
        public IActionResult Create(Recipe recipe)
        {
             if (ModelState.IsValid) 
            {
                recipe.Id = _recipes.Count > 0 ? _recipes.Max(r => r.Id) + 1 : 1;
                _recipes.Add(recipe);
                return RedirectToAction(nameof(Index));
            }
            return View(recipe);
        }

      
        public IActionResult Delete(int id)
        {
            var recipe = _recipes.FirstOrDefault(r => r.Id == id);
            if (recipe != null) _recipes.Remove(recipe);
            return RedirectToAction(nameof(Index));
        }
    }
}