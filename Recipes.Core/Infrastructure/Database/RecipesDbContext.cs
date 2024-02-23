using System.Diagnostics;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recipes.Core.Application;
using Recipes.Core.Domain;

namespace Recipes.Core.Infrastructure.Database;

public class RecipesDbContext : IdentityDbContext<ApplicationUser>, IRecipesDbContext
{
    public RecipesDbContext(DbContextOptions<RecipesDbContext> options) : base(options)
    {
    }
    
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<Group> Groups => Set<Group>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}