using Microsoft.EntityFrameworkCore;
using Recipes.Core.Domain;

namespace Recipes.Core.Application;

public interface IRecipesDbContext
{
    DbSet<ApplicationUser> Users { get; }
    DbSet<Recipe> Recipes { get; }
    DbSet<Ingredient> Ingredients { get; }
    DbSet<Group> Groups { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}