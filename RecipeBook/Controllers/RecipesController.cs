using Microsoft.AspNetCore.Mvc;
using RecipeBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

public class RecipesController : Controller
{
    private readonly RecipeBookContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public RecipesController(UserManager<ApplicationUser> userManager, RecipeBookContext context)
    {
        _userManager = userManager;
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
    public async Task<ActionResult> Create(RecipeViewModel model)
    {
        if (ModelState.IsValid)
        {
            Recipe recipe = new Recipe
            {
                Name = model.RecipeName,
                Instructions = model.Instructions,
                Description = model.Description,
                ImageUrl = model.ImageUrl
            };

            foreach (var ingredientViewModel in model.Ingredients)
            {
                string ingredientName = ingredientViewModel.Name.Trim();
                Ingredient existingIngredient = _context.Ingredients.FirstOrDefault(i => i.Name == ingredientName);
                Ingredient ingredient;
                if (existingIngredient != null)
                {
                    ingredient = existingIngredient;
                }
                else
                {
                    ingredient = new Ingredient { Name = ingredientName };
                    _context.Ingredients.Add(ingredient);
                }
                IngredientRecipe ingredientRecipe = new IngredientRecipe { Ingredient = ingredient, Recipe = recipe, Quantity = ingredientViewModel.Quantity };
                recipe.IRJoin.Add(ingredientRecipe);
                _context.IngredientRecipes.Add(ingredientRecipe);
            }

            foreach (var tagViewModel in model.Tags)
            {
                string tagName = tagViewModel.Name.Trim(); // Trim to handle leading/trailing whitespaces

                // Check if the tag with the same name already exists
                Tag existingTag = _context.Tags.FirstOrDefault(t => t.Name == tagName);

                Tag tag;

                if (existingTag != null)
                {
                    // If the tag exists, reuse it
                    tag = existingTag;
                }
                else
                {
                    // If the tag doesn't exist, create a new one
                    tag = new Tag { Name = tagName };
                    _context.Tags.Add(tag);
                }

                RecipeTag recipeTag = new RecipeTag { Recipe = recipe, Tag = tag };
                recipe.RTJoin.Add(recipeTag);
                _context.RecipeTags.Add(recipeTag);
            }
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
            recipe.User = currentUser;
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
            .Include(rec => rec.RTJoin)
            .ThenInclude(join => join.Tag)
            .FirstOrDefault(r => r.RecipeId == id);
        return View(rec);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        Recipe recipe = _context.Recipes
            .Include(r => r.IRJoin)
            .ThenInclude(join => join.Ingredient)
            .Include(r => r.RTJoin)
            .ThenInclude(join => join.Tag)
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
            // Retrieve the existing recipe from the database including related entities
            Recipe existingRecipe = _context.Recipes
                .Include(r => r.IRJoin)
                .ThenInclude(join => join.Ingredient)
                .Include(r => r.RTJoin)
                .ThenInclude(join => join.Tag)
                .FirstOrDefault(r => r.RecipeId == id);

            if (existingRecipe == null)
            {
                return NotFound();
            }

            // Clear existing IngredientRecipe relationships for the recipe
            _context.IngredientRecipes.RemoveRange(existingRecipe.IRJoin);

            // Clear existing RecipeTag relationships for the recipe
            _context.RecipeTags.RemoveRange(existingRecipe.RTJoin);

            // Update properties of the existing recipe
            existingRecipe.Name = model.Name;
            existingRecipe.Description = model.Description;
            existingRecipe.Instructions = model.Instructions;
            existingRecipe.ImageUrl = model.ImageUrl;
            existingRecipe.Rating = model.Rating;

            // Add new IngredientRecipe entities
            foreach (var modelJoin in model.IRJoin)
            {
                // If the join has a new Ingredient, add it only if it doesn't already exist
                string trimmedIngredientName = modelJoin.Ingredient.Name.Trim();
                Ingredient existingIngredient = _context.Ingredients.FirstOrDefault(i => i.Name == trimmedIngredientName);

                if (existingIngredient != null)
                {
                    // If the ingredient already exists, associate it with the join
                    modelJoin.Ingredient = existingIngredient;
                }
                else
                {
                    // If the ingredient doesn't exist, add a new one
                    Ingredient newIngredient = new Ingredient { Name = trimmedIngredientName };
                    modelJoin.Ingredient = newIngredient;
                    _context.Ingredients.Add(newIngredient);
                }

                // Add new IngredientRecipe
                existingRecipe.IRJoin.Add(new IngredientRecipe
                {
                    Ingredient = modelJoin.Ingredient,
                    Recipe = existingRecipe,
                    Quantity = modelJoin.Quantity
                });
            }

            // Add new RecipeTag entities
            foreach (var modelTag in model.RTJoin)
            {
                // If the join has a new Tag, add it only if it doesn't already exist
                string trimmedTagName = modelTag.Tag.Name.Trim();
                Tag existingTag = _context.Tags.FirstOrDefault(t => t.Name == trimmedTagName);

                if (existingTag != null)
                {
                    // If the tag already exists, associate it with the join
                    modelTag.Tag = existingTag;
                }
                else
                {
                    // If the tag doesn't exist, add a new one
                    Tag newTag = new Tag { Name = trimmedTagName };
                    modelTag.Tag = newTag;
                    _context.Tags.Add(newTag);
                }

                // Add new RecipeTag
                existingRecipe.RTJoin.Add(new RecipeTag
                {
                    Tag = modelTag.Tag,
                    Recipe = existingRecipe
                });
            }

            // Save changes to the database
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