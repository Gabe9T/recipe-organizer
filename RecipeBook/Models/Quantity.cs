using System.Collections.Generic;

namespace RecipeBook.Models;

public class Quantity
{
    public int QuantityId { get; set; }
    public string Amount { get; set; }
    public List<IngredientQuantity> JoinEntities { get; }
}