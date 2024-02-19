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
    
    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}