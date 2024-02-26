using Recipes.Core.Domain;

namespace Recipes.Api.Models.Requests;

public class UpdateRecipeRequest
{
    public string Name { get; set; } = null!;
    public string Instructions { get; set; } = null!;
    public Diet? Diet { get; set; }
    public Course? Course { get; set; }
}