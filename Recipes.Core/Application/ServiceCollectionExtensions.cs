using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recipes.Core.Application.Auth;

namespace Recipes.Core.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<IAuthenticator, Authenticator>();
        serviceCollection.AddSingleton<IAuthTokenGenerator, JwtGenerator>();
        
        serviceCollection.Configure<UserSettings>(configuration.GetRequiredSection("User"));
        
        serviceCollection.AddSingleton<IIngredientsService, CachedIngredientsService>();

        var assemblies = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(x => x.FullName!.StartsWith(nameof(Recipes)))
            .ToArray();
        
        serviceCollection.AddMediatR(c => c.RegisterServicesFromAssemblies(assemblies));
        serviceCollection.AddAutoMapper(assemblies);
        serviceCollection.AddValidatorsFromAssemblies(assemblies);
        
        return serviceCollection;
    }
}