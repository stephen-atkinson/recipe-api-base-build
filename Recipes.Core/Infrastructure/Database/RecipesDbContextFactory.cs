using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Sqlite.Scaffolding.Internal;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Recipes.Core.Infrastructure.Database;

public class RecipesDbContextFactory : IDbContextFactory<RecipesDbContext>
{
    private readonly DbContextOptions<RecipesDbContext> _dbContextOptions;

    public RecipesDbContextFactory(IServiceProvider serviceProvider, IHostEnvironment hostEnvironment, IOptions<RecipesDbSettings> dbOptions)
    {
        Debugger.Launch();
        
        var dbPath = Path.Join(hostEnvironment.ContentRootPath, dbOptions.Value.FileName);

        var model = SqliteConventionSetBuilder.CreateModelBuilder()
            .ApplyConfigurationsFromAssembly(GetType().Assembly)
            .FinalizeModel();
        
        _dbContextOptions = new DbContextOptionsBuilder<RecipesDbContext>()
            .UseSqlite($"Data Source={dbPath}")
            .UseApplicationServiceProvider(serviceProvider)
            .UseModel(model)
            .Options;
    }
    
    public RecipesDbContext CreateDbContext() => new (_dbContextOptions);
}