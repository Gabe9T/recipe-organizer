using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeBook.Models;

public class Recipe
{
    public int RecipeId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Instructions { get; set; }
    public string ImageUrl { get; set; }
    public int Rating { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public List<IngredientRecipe> IRJoin { get; } = new List<IngredientRecipe>();
    public List<RecipeTag> RTJoin { get; } = new List<RecipeTag>();

}