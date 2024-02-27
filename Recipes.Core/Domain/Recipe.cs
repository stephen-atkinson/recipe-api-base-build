namespace Recipes.Core.Domain;

public class Recipe
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Instructions { get; set; }
    public int Difficulty { get; set; }
    public Diet? Diet { get; set; }
    public Course? Course { get; set; }
    public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    public string UserId { get; set; } = null!;
}