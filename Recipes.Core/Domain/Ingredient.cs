namespace Recipes.Core.Domain;

public class Ingredient
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string SupplierName { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string ExternalId { get; set; } = null!;
    public Recipe Recipe { get; set; } = null!;
}