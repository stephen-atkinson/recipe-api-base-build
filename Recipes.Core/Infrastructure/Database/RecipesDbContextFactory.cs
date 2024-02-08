using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Recipes.Core.Infrastructure.Database;

public class RecipesDbContextFactory : IDbContextFactory<RecipesDbContext>
{
    private readonly DbContextOptions<RecipesDbContext> _dbContextOptions;

    public RecipesDbContextFactory(IHostEnvironment hostEnvironment, IOptions<RecipesDbSettings> dbOptions, ILoggerFactory loggerFactory)
    {
        var dbPath = Path.Join(hostEnvironment.ContentRootPath, dbOptions.Value.FileName);

        var model = SqliteConventionSetBuilder.CreateModelBuilder()
            .ApplyConfigurationsFromAssembly(typeof(RecipesDbContext).Assembly)
            .FinalizeModel();
        
        _dbContextOptions = new DbContextOptionsBuilder<RecipesDbContext>()
            .UseSqlite($"Data Source={dbPath}")
            .UseLoggerFactory(loggerFactory)
            .UseModel(model)
            .Options;
    }
    
    public RecipesDbContext CreateDbContext() => new (_dbContextOptions);
}