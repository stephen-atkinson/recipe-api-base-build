using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;

namespace Recipes.Api.Options;

public class ApiExplorerConfigOptions : IConfigureOptions<ApiExplorerOptions>
{
    public void Configure(ApiExplorerOptions options)
    {
        // ReSharper disable once StringLiteralTypo
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    }
}