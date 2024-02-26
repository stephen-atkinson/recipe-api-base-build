using Recipes.Core.Domain;

namespace Recipes.Api.Models.Results;

public class GroupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public Diet? Diet { get; set; }
    public Course? Course { get; set; }
    public ICollection<string> RecipeIds { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime LastUpdated { get; set; }
    public string CreatedBy { get; set; } = null!;
}