namespace Recipes.Core.Domain;

public class Recipe
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Instructions { get; set; } = null!;
    public Course Course { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastUpdated { get; set; }
    public ICollection<Ingredient> Ingredients { get; set; } = null!;
    public ApplicationUser ApplicationUser { get; set; } = null!;
}