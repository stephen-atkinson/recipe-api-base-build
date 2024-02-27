namespace Recipes.Core.Application.Contracts;

public interface IIngredientsApi
{
    Task<bool> IsHealthyAsync(CancellationToken cancellationToken);
}