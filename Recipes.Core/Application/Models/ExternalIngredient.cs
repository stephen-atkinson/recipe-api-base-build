namespace Recipes.Core.Application.Models;

public record ExternalIngredient
{
    public string Id { get; set; }
    public string Category { get; set; }
    public string SupplierFriendlyName { get; set; }
    public string Description { get; set; }
    public decimal Cost { get; set; }
}