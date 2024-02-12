namespace Recipe.Models;

public class Quantity
{
    public int QuanityId { get; set; }
    public string Amount { get; set; }
    public List<IngredientQuantity> JoinEntities { get; }
}