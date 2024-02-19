using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Recipes.Api.Options;

public class SwaggerUiConfigOptions : IConfigureOptions<SwaggerUIOptions>
{
    private readonly IApiDescriptionGroupCollectionProvider _apiDescriptionGroupCollectionProvider;

    public SwaggerUiConfigOptions(IApiDescriptionGroupCollectionProvider apiDescriptionGroupCollectionProvider)
    {
        _apiDescriptionGroupCollectionProvider = apiDescriptionGroupCollectionProvider;
    }
    
    public void Configure(SwaggerUIOptions options)
    {
        // build a swagger endpoint for each discovered API version
        foreach (var description in _apiDescriptionGroupCollectionProvider.ApiDescriptionGroups.Items)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName!.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    }
}