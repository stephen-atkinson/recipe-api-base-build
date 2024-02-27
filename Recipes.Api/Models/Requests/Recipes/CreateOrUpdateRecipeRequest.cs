using Recipes.Core.Domain;

namespace Recipes.Api.Models.Requests.Recipes;

public class CreateOrUpdateRecipeRequest
{
    public string Name { get; set; } = null!;
    public string Instructions { get; set; } = null!;
    public int Difficulty { get; set; }
    public Diet? Diet { get; set; }
    public Course? Course { get; set; }
}