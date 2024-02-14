namespace RecipeBook.Models;
using System.ComponentModel.DataAnnotations;

public class Rating
{
    public int RatingId { get; set; }
    public int Value { get; set; }
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; }
}