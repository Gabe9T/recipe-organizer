using System.Collections.Generic;

namespace Recipe.Models
{

    public class Ingredient
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public List<IngredientQuantity> IQJoin { get; }
        public List<IngredientRecipe> IRJoin { get; }
    }
}