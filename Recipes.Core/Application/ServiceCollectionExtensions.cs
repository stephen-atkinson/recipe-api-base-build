using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Recipes.Core.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
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