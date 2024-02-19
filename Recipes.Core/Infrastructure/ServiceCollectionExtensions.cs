using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Recipes.Core.Application;
using Recipes.Core.Domain;
using Recipes.Core.Infrastructure.Database;
using Recipes.Core.Infrastructure.Ingredients;

namespace Recipes.Core.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        
        serviceCollection.Configure<RecipesDbSettings>(configuration.GetRequiredSection("IngredientsApi"));
        serviceCollection.AddHttpClient<IIngredientsApi, IngredientsApi>();

        serviceCollection.AddMemoryCache();

        serviceCollection.Configure<RecipesDbSettings>(configuration.GetRequiredSection("RecipesDb"));
        serviceCollection.AddDbContext<RecipesDbContext>((sp, b) =>
        {
            var hostEnvironment = sp.GetRequiredService<IHostEnvironment>();
            var dbOptions = sp.GetRequiredService<IOptions<RecipesDbSettings>>();

            var dbPath = Path.Join(hostEnvironment.ContentRootPath, dbOptions.Value.FileName);

            b.UseSqlite($"Data Source={dbPath}");
        });

        serviceCollection.AddScoped<IRecipesDbContext>(sp => sp.GetRequiredService<RecipesDbContext>());

        serviceCollection.AddIdentityCore<ApplicationUser>()
            .AddUserManager<AspNetUserManager<ApplicationUser>>()
            .AddSignInManager()
            .AddEntityFrameworkStores<RecipesDbContext>();

        return serviceCollection;
    }
}