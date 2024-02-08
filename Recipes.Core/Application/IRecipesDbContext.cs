using Microsoft.EntityFrameworkCore;
using Recipes.Core.Domain;

namespace Recipes.Core.Application;

public interface IRecipesDbContext
{
    DbSet<Recipe> Recipes { get; }
    DbSet<Ingredient> Ingredients { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}