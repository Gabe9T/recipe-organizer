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

        public List<string> Ingredients { get; set; } = new List<string>();
    }
}