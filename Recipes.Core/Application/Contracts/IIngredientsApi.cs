using System.Runtime.InteropServices;
using Recipes.Core.Application.Models;

namespace Recipes.Core.Application.Contracts;

public interface IIngredientsApi
{
    Task<bool> IsHealthyAsync(CancellationToken cancellationToken);
    
    Task<IReadOnlyCollection<ExternalIngredient>> BatchGet(BatchGetIngredientsRequest request, CancellationToken cancellationToken);
}