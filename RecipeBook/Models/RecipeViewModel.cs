using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeBook.Models
{
    public class RecipeViewModel
    {
        [Required(ErrorMessage = "Recipe Name is required")]
        public string RecipeName { get; set; }

        [Required(ErrorMessage = "Instructions are required")]
        public string Instructions { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }
        public string UserId { get; set; }
        public List<IngredientViewModel> Ingredients { get; set; } = new List<IngredientViewModel>();
        public List<TagViewModel> Tags { get; set; } = new List<TagViewModel>();
    }

    public class IngredientViewModel
    {
        [Required(ErrorMessage = "Ingredient Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        public string Quantity { get; set; }
    }
    public class TagViewModel
    {
        [Required(ErrorMessage = "Tag Name is required")]
        public string Name { get; set; }
    }
}