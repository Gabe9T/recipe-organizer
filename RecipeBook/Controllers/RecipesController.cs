using Microsoft.AspNetCore.Mvc;
using RecipeBook.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class RecipesController : Controller
{
    private readonly RecipeBookContext _context;

    public RecipesController(RecipeBookContext context)
    {
        _context = context;
    }

    public ActionResult Index()
    {
        List<Recipe> model = _context.Recipes.ToList();
        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new RecipeViewModel());
    }

    [HttpPost]
    public IActionResult Create(RecipeViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Create and save the recipe and ingredients to the database
            Recipe recipe = new Recipe
            {
                Name = model.RecipeName,
                Instructions = model.Instructions,
                Description = model.Description,
                ImageUrl = model.ImageUrl
            };

            foreach (var ingredientViewModel in model.Ingredients)
            {
                Ingredient ingredient = new Ingredient { Name = ingredientViewModel.Name };
                IngredientRecipe ingredientRecipe = new IngredientRecipe { Ingredient = ingredient, Recipe = recipe, Quantity = ingredientViewModel.Quantity };
                recipe.IRJoin.Add(ingredientRecipe);
                _context.Ingredients.Add(ingredient);
                _context.IngredientRecipes.Add(ingredientRecipe);
            }

            _context.Recipes.Add(recipe);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        return View(model);
    }

    public ActionResult Details(int id)
    {
        Recipe rec = _context.Recipes
            .Include(r => r.IRJoin)
            .ThenInclude(join => join.Ingredient)
            .FirstOrDefault(r => r.RecipeId == id);
        return View(rec);
    }

[HttpGet]
public IActionResult Edit(int id)
{
    Recipe recipe = _context.Recipes
        .Include(r => r.IRJoin)
        .ThenInclude(join => join.Ingredient)
        .FirstOrDefault(r => r.RecipeId == id);

    if (recipe == null)
    {
        return NotFound();
    }

    return View(recipe);
}

[HttpPost]
public IActionResult Edit(int id, Recipe model)
{

    if (ModelState.IsValid)
    {
        // Update the recipe in the database
        _context.Entry(model).State = EntityState.Modified;
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    return View(model);
}

[HttpGet]
public IActionResult Delete(int id)
{
    Recipe recipe = _context.Recipes.Find(id);
    if (recipe == null)
    {
        return NotFound();
    }

    return View(recipe);
}

[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public IActionResult DeleteConfirmed(int id)
{
    Recipe recipe = _context.Recipes.Find(id);
    if (recipe == null)
    {
        return NotFound();
    }

    _context.Recipes.Remove(recipe);
    _context.SaveChanges();

    return RedirectToAction("Index");
}
}