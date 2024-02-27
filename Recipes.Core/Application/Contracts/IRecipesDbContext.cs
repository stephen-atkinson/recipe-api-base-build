using Microsoft.EntityFrameworkCore;
using Recipes.Core.Domain;

namespace Recipes.Core.Application.Contracts;

public interface IRecipesDbContext
{
    DbSet<Recipe> Recipes { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}