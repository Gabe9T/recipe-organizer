using Microsoft.AspNetCore.Mvc;
using RecipeBook.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace RecipeBook.Controllers;
public class SearchController : Controller
{
    private readonly RecipeBookContext _db;
    public SearchController(RecipeBookContext db)
    {
        _db = db;
    }

    public async Task<ActionResult> Index(string searchType, string searchTerm)
    {
        switch (searchType)
        {
            case "rec":
                List<Recipe> recResults = await _db.Recipes
                    .Where(r => r.Name.Contains(searchTerm))
                    .ToListAsync();
                return View(recResults);
            case "ing":
                List<Ingredient> ingResults = await _db.Ingredients
                    .Where(i => i.Name.Contains(searchTerm))
                    .ToListAsync();
                return View(ingResults);
            default:
                List<Recipe> ingRecResults = await _db.Recipes
                    .Where(recipe => recipe.IRJoin.Any(join => join.Ingredient.Name.Contains(searchTerm)))
                    .ToListAsync();
                return View(ingRecResults);
                // Ingredient ing = _db.Ingredients.FirstOrDefault(i => i.Name == searchTerm);
                // List<Recipe> ingRecResults = _db.Recipes
                //     .Include(ir => ir.IRJoin)
                //     .ThenInclude(join => join.Ingredient)
                //     .Where(i => i.IngredientName == searchTerm)

                // List<Recipe> ingRecResults = _db.Recipes
                //     .Include(r => r.IRJoin)
                //     .ThenInclude(j => j.IngredientId)
        }
    }
}
