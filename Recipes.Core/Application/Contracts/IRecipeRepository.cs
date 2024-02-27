using Recipes.Core.Application.Models;
using Recipes.Core.Domain;

namespace Recipes.Core.Application.Contracts;

public interface IRecipeRepository
{
    /// <returns>The id of the recipe.</returns>
    Task<int> CreateAsync(Recipe recipe, CancellationToken cancellationToken);
    Task UpdateAsync(Recipe recipe, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task<Recipe?> GetAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Recipe>> GetAsync(GetRecipesCriteria criteria, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);
}