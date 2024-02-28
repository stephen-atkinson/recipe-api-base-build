namespace Recipes.Core.Application.Models;

public record BatchGetIngredientsRequest
{
    public IReadOnlyCollection<string> Ids { get; set; }
}