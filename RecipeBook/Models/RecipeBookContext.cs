using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RecipeBook.Models
{
    public class RecipeBookContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Quantity> Quantities { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<IngredientQuantity> IngredientQuantities { get; set; }
        public DbSet<IngredientRecipe> IngredientRecipes { get; set; }
        public DbSet<RecipeTag> RecipeTags { get; set; }

        public RecipeBookContext(DbContextOptions options) : base(options) { }
    }
}