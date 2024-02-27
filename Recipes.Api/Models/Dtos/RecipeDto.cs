using Recipes.Core.Domain;

namespace Recipes.Api.Models.Dtos;

public class RecipeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Instructions { get; set; } = null!;
    public int Difficulty { get; set; }
    public decimal AverageRating { get; set; }
    public Diet? Diet { get; set; }
    public Course? Course { get; set; }
    public string UserId { get; set; } = null!;
}