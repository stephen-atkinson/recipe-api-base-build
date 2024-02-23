using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Recipes.Core.Infrastructure.Database;

public class RecipesDbContextOptionsFactory : IRecipesDbContextOptionsFactory
{
    private readonly IServiceProvider _serviceProvider;

    public RecipesDbContextOptionsFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public DbContextOptions<RecipesDbContext> Create()
    {
        var builder = new DbContextOptionsBuilder<RecipesDbContext>()
            .UseApplicationServiceProvider(_serviceProvider);

        ConfigureDatabaseProvider(builder);

        return builder.Options;
    }

    protected virtual void ConfigureDatabaseProvider(DbContextOptionsBuilder builder)
    {
        var hostEnvironment = _serviceProvider.GetRequiredService<IHostEnvironment>();
        var dbOptions = _serviceProvider.GetRequiredService<IOptions<RecipesDbSettings>>();

        var dbPath = Path.Join(hostEnvironment.ContentRootPath, dbOptions.Value.DatabaseName);

        builder.UseSqlite($"Data Source={dbPath}");
    }
}