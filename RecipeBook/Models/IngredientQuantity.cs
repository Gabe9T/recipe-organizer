namespace RecipeBook.Models;

public class IngredientQuantity
{
    public int IngredientQuantityId { get; set; }
    public int IngredientId { get; set; }
    public Ingredient Ingredient { get; set; }
    public int QuantityId { get; set; }
    public Quantity Quantity { get; set; }
}