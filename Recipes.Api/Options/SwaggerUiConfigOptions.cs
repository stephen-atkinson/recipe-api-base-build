using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Recipes.Api.Options;

public class SwaggerUiConfigOptions : IConfigureOptions<SwaggerUIOptions>
{
    private readonly IEndpointRouteBuilder _endpointRouteBuilder;

    public SwaggerUiConfigOptions(IEndpointRouteBuilder endpointRouteBuilder)
    {
        _endpointRouteBuilder = endpointRouteBuilder;
    }
    
    public void Configure(SwaggerUIOptions options)
    {
        var descriptions = _endpointRouteBuilder.DescribeApiVersions();

        // build a swagger endpoint for each discovered API version
        foreach (var description in descriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    }
}