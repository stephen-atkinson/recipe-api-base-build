using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Recipes.Api.OpenApi;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSwaggerGen();
        serviceCollection.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigOptions>();
        serviceCollection.AddApiVersioning(opt =>
            {
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
            }).AddApiExplorer(options =>
            {
                // ReSharper disable once StringLiteralTypo
                options.GroupNameFormat = "'v'VVV";
            })
            .AddMvc();

        return serviceCollection;
    }
}