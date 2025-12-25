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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Category,Ingredients,Instructions")] Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                // Tarihi otomatik şu anki zaman yapalım
                recipe.CreatedDate = DateTime.Now;

                // Kategori boş gelirse varsayılan ata
                if (string.IsNullOrEmpty(recipe.Category))
                {
                    recipe.Category = "Ana Yemek";
                }

                _context.Add(recipe);
                await _context.SaveChangesAsync();
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