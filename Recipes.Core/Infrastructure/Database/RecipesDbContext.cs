using Microsoft.EntityFrameworkCore;
using Recipes.Core.Application;
using Recipes.Core.Domain;

namespace Recipes.Core.Infrastructure.Database;

public class RecipesDbContext : DbContext, IRecipesDbContext
{
    public RecipesDbContext(DbContextOptions<RecipesDbContext> options) : base(options)
    {
    }
    
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
}