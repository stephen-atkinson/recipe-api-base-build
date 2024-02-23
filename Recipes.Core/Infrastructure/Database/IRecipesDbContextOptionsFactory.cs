using Microsoft.EntityFrameworkCore;

namespace Recipes.Core.Infrastructure.Database;

public interface IRecipesDbContextOptionsFactory
{
    public DbContextOptions<RecipesDbContext> Create();
}