namespace Recipes.Api.Versions.V2.Models.Dtos;

public record IngredientDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string SupplierName { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string ExternalId { get; set; } = null!;
}