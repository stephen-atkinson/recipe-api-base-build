using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recipes.Core.Application.Contracts;
using Recipes.Core.Infrastructure.Database;
using Recipes.Core.Infrastructure.Ingredients;

namespace Recipes.Core.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        
        serviceCollection.Configure<IngredientsApiSettings>(configuration.GetRequiredSection("IngredientsApi"));
        serviceCollection.AddHttpClient<IIngredientsApi, IngredientsApi>();

        serviceCollection.AddMemoryCache();

        serviceCollection.Configure<RecipesDbSettings>(configuration.GetRequiredSection("RecipesDb"));

        serviceCollection.AddSingleton<IRecipesDbContextOptionsFactory, RecipesDbContextOptionsFactory>();
        serviceCollection.AddScoped<DbContextOptions<RecipesDbContext>>(sp =>
            sp.GetRequiredService<IRecipesDbContextOptionsFactory>().Create());
        serviceCollection.AddDbContext<RecipesDbContext>();
        
        serviceCollection.AddScoped<IRecipesDbContext>(sp => sp.GetRequiredService<RecipesDbContext>());

        serviceCollection.AddIdentityCore<IdentityUser>()
            .AddUserManager<AspNetUserManager<IdentityUser>>()
            .AddSignInManager()
            .AddEntityFrameworkStores<RecipesDbContext>();

        return serviceCollection;
    }
}