namespace Recipes.Core.Domain;

public class Recipe
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Instructions { get; set; } = null!;
    public Diet? Diet { get; set; }
    public Course? Course { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastUpdated { get; set; }
    public ICollection<Ingredient> Ingredients { get; set; } = null!;
    public ApplicationUser ApplicationUser { get; set; } = null!;
    public ICollection<Group> Groups { get; set; } = null!;
}