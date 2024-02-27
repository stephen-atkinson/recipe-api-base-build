namespace Recipes.Core.Domain;

public class Rating
{
    public decimal Value { get; set; }
    public string UserId { get; set; } = null!;
}