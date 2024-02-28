using Recipes.Core.Domain;

namespace Recipes.Api.Versions.V2.Models.Requests.Recipes;

public class CreateOrUpdateRecipeRequest
{
    public string Name { get; set; } = null!;
    public string Instructions { get; set; } = null!;
    public int Difficulty { get; set; }
    public Diet? Diet { get; set; }
    public Course? Course { get; set; }
    public IReadOnlyCollection<string> IngredientIds { get; set; }
}