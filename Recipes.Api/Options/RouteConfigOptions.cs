using Microsoft.Extensions.Options;

namespace Recipes.Api.Options;

public class RouteConfigOptions : IConfigureOptions<RouteOptions>
{
    public void Configure(RouteOptions options)
    {
        options.LowercaseUrls = true;
    }
}