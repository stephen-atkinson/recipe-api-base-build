using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recipes.Core.Application;
using Recipes.Core.Infrastructure.Database;
using Recipes.Core.Infrastructure.Ingredients;

namespace Recipes.Core.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.Configure<RecipesDbSettings>(configuration.GetRequiredSection("IngredientsApi"));
        serviceCollection.AddHttpClient<IIngredientsApi, IngredientsApi>();

        serviceCollection.Configure<RecipesDbSettings>(configuration.GetRequiredSection("RecipesDb"));
        serviceCollection.AddDbContextFactory<RecipesDbContext, RecipesDbContextFactory>();
        serviceCollection.AddScoped<IRecipesDbContext>(s => s.GetRequiredService<IDbContextFactory<RecipesDbContext>>().CreateDbContext());
        
        serviceCollection.AddMemoryCache();
        
        return serviceCollection;
    }
}