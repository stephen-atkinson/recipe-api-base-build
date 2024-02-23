namespace Recipes.Core.Application;

public interface IIngredientsApi
{
    Task<bool> IsHealthyAsync(CancellationToken cancellationToken);
}