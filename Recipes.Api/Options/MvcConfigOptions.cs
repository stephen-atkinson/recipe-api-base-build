using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;

namespace Recipes.Api.Options;

public class MvcConfigOptions : IConfigureOptions<MvcOptions>
{
    public void Configure(MvcOptions options)
    {
        options.OutputFormatters.RemoveType<StringOutputFormatter>();
    }
}