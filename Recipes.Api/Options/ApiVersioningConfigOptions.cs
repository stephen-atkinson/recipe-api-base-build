using Asp.Versioning;
using Microsoft.Extensions.Options;

namespace Recipes.Api.Options;

public class ApiVersioningConfigOptions : IConfigureOptions<ApiVersioningOptions>
{
    public void Configure(ApiVersioningOptions options)
    {
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = new QueryStringApiVersionReader();
    }
}