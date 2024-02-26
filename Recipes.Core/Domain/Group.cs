namespace Recipes.Core.Domain;

public class Group
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public Diet? Diet { get; set; }
    public Course? Course { get; set; }
    public ICollection<Recipe> Recipes { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime LastUpdated { get; set; }
    public ApplicationUser ApplicationUser { get; set; } = null!;
}