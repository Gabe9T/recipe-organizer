using System.Collections.Generic;

namespace RecipeBook.Models
{

    public class Ingredient
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public List<IngredientQuantity> IQJoin { get; } = new List<IngredientQuantity>();
        public List<IngredientRecipe> IRJoin { get; } = new List<IngredientRecipe>();

    }
}