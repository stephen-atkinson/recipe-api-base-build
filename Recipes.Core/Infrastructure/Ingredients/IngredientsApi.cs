using Microsoft.Extensions.Options;
using Recipes.Core.Application;

namespace Recipes.Core.Infrastructure.Ingredients;

public class IngredientsApi : IIngredientsApi
{
    private readonly IOptions<IngredientsApiSettings> _options;
    private readonly HttpClient _httpClient;

    public IngredientsApi(IOptions<IngredientsApiSettings> options, HttpClient httpClient)
    {
        _options = options;
        _httpClient = httpClient;
    }
}