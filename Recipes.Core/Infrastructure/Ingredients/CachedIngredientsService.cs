using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Recipes.Core.Application.Contracts;
using Recipes.Core.Application.Models;
using Recipes.Core.Domain;

namespace Recipes.Core.Infrastructure.Ingredients;

public class CachedIngredientsService : IIngredientsService
{
    private readonly IIngredientsApi _api;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<CachedIngredientsService> _logger;

    public CachedIngredientsService(IIngredientsApi api, IMemoryCache memoryCache, ILogger<CachedIngredientsService> logger)
    {
        _api = api;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<ExternalIngredient>> BatchGet(IReadOnlyCollection<string> ids, CancellationToken cancellationToken)
    {
        var ingredients = new List<ExternalIngredient>();

        var ingredientIdsToGet = new List<string>();
        
        foreach (var id in ids)
        {
            var ingredient = _memoryCache.Get<ExternalIngredient>(id);

            if (ingredient == null)
            {
                ingredientIdsToGet.Add(id);
            }
            else
            {
                ingredients.Add(ingredient);
            }
        }

        if (!ingredientIdsToGet.Any())
        {
            return ingredients;
        }

        var request = new BatchGetIngredientsRequest { Ids = ingredientIdsToGet };
        
        _logger.LogInformation("No cache found for ingredient ids. Ids: {Ids}", string.Join(", ", request.Ids));

        var uncachedIngredients = await _api.BatchGet(request, cancellationToken);

        foreach (var ingredient in uncachedIngredients)
        {
            ingredients.Add(ingredient);

            _memoryCache.Set(ingredient.Id, ingredient, DateTimeOffset.UtcNow.AddMinutes(1));
        }

        return ingredients;
    }
}