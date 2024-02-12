namespace Recipes.Core.Domain;

public class Recipe
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public Course Course { get; set; }
    public ICollection<Ingredient> Ingredients { get; set; } = null!;
    public User User { get; set; } = null!;
}