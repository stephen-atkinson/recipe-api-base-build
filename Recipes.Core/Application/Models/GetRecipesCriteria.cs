using Recipes.Core.Domain;

namespace Recipes.Core.Application.Models;

public record GetRecipesCriteria
{
    public int Skip { get; set; }
    public int Take { get; set; } = 20;
    public int? DifficultyFrom { get; set; }
    public int? DifficultyTo { get; set; }
    public Diet? Diet { get; set; }
    public Course? Course { get; set; }
    public IReadOnlyCollection<int>? Ids { get; set; }
    public string? UserId { get; set; }
}