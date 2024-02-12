// using Microsoft.AspNetCore.Identity;
// using System.Collections.Generic;
// using Microsoft.AspNetCore.Mvc;
// using System.Threading.Tasks;
// using System.Security.Claims;
// using RecipeBook.Models;
// using System.Linq;

// namespace RecipeBook.Controllers
// {
//     public class RecipesController : Controller
//     {
//         private readonly RecipeBookContext _db;
//         private readonly UserManager<ApplicationUser> _userManager;
//         public RecipesController(UserManager<ApplicationUser> userManager, RecipeBookContext db)
//         {
//             _userManager = userManager;
//             _db = db;
//         }

//         public ActionResult Create()
//         {
//             View()
//         }

//         [HttpPost]
//         public ActionResult Create(Recipe rec)
//         {
//             if (!ModelState.IsValue)
//             {
//                 return View();
//             }
//             else
//             {
//                 //Ingredient newIng =new(rec.RecipeId, ing-name, ing-url) 
//                 _db.Ingredients.Add(ing);
//                 _db.Recipes.Add(rec);
//                 // await add join table entries?


//                 _db.SaveChanges();
//                 return RedirectToAction("Create", "Ingredients");
//             }
//         }
//     }
// }

using Microsoft.AspNetCore.Mvc;
using RecipeBook.Models;
using Microsoft.Extensions.Logging;

public class RecipesController : Controller
{
    private readonly RecipeBookContext _context;

    private readonly ILogger<RecipesController> _logger;
    public RecipesController()

    public RecipesController(RecipeBookContext context)
    {
        _context = context;
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
                // Other properties
            };

            foreach (string ingredientName in model.Ingredients)
            {
                Ingredient ingredient = new Ingredient { Name = ingredientName };
                IngredientRecipe ingredientRecipe = new IngredientRecipe { Ingredient = ingredient, Recipe = recipe };
                recipe.IRJoin.Add(ingredientRecipe);
                _context.Ingredients.Add(ingredient);
                _context.IngredientRecipes.Add(ingredientRecipe);
            }

            _context.Recipes.Add(recipe);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        return View(model);
    }

    // Other actions and methods as needed
}