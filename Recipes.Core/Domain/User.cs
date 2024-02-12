namespace Recipes.Core.Domain;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public ICollection<Recipe> Recipes { get; set; } = null!;
}