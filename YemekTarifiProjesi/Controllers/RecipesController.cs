using Microsoft.AspNetCore.Mvc;
using YemekTarifiProjesi.Models;

namespace YemekTarifiProjesi.Controllers
{
    public class RecipesController : Controller
    {
      
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