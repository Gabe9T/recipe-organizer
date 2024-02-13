using System.Collections.Generic;

namespace RecipeBook.Models;

public class Tag
{
    public int TagId { get; set; }
    public string Name { get; set; }
    public List<RecipeTag> RTJoin { get; } = new List<RecipeTag>();
}