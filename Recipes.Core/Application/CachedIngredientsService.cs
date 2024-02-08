using Microsoft.Extensions.Caching.Memory;

namespace Recipes.Core.Application;

public class CachedIngredientsService : IIngredientsService
{
    private readonly IIngredientsApi _api;
    private readonly IMemoryCache _memoryCache;

    public CachedIngredientsService(IIngredientsApi api, IMemoryCache memoryCache)
    {
        _api = api;
        _memoryCache = memoryCache;
    }
}