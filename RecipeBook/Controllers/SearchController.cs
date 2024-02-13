using Microsoft.AspNetCore.Mvc;
using RecipeBook.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace RecipeBook.Controllers;
public class SearchController : Controller
{
    private readonly RecipeBookContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public SearchController(UserManager<ApplicationUser> userManager, RecipeBookContext db)
    {
        _userManager = userManager;
        _db = db;
    }

    public async Task<ActionResult> Index(string searchType, string searchTerm)
    {
        switch (searchType)
        {
            case "rec":
                List<Recipes> recResults = await _db.Recipes
                    .Where(r => r.Name.Contains(searchTerm))
                    .ToListAsync();
                return View(recResults);
            case "ing":
                List<Ingredients> ingResults = await _db.Recipes
                    .Where(i => i.Name.Contains(searchTerm))
                    .ToListAsync();
                return View(ingResults);
            default:
                List<Recipe> ingRecResults = await _db.Recipes
                    .Where(recipe => recipe.IRJoin.Any(join => join.Ingredient.Name == searchTerm))
                    .ToListAsync();
                return View(ingRecResults)
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
