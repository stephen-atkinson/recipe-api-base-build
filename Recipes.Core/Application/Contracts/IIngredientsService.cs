using Recipes.Core.Application.Models;

namespace Recipes.Core.Application.Contracts;

public interface IIngredientsService
{
    Task<IReadOnlyCollection<ExternalIngredient>> BatchGet(IReadOnlyCollection<string> ids, CancellationToken cancellationToken);
}