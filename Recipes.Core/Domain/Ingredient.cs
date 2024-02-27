namespace Recipes.Core.Domain;

public class Ingredient
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string SupplierName { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string ExternalId { get; set; } = null!;
}