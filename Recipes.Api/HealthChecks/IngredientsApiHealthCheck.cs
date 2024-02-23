using Microsoft.Extensions.Diagnostics.HealthChecks;
using Recipes.Core.Application;

namespace Recipes.Api.HealthChecks;

public class IngredientsApiHealthCheck : IHealthCheck
{
    private readonly IIngredientsApi _ingredientsApi;

    public const string HealthCheckName = "Ingredients API";

    public IngredientsApiHealthCheck(IIngredientsApi ingredientsApi)
    {
        _ingredientsApi = ingredientsApi;
    }
    
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
    {
        try
        {
            var healthy = await _ingredientsApi.IsHealthyAsync(cancellationToken);

            return healthy ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Failed to ping the ingredients API.", ex);
        }
    }
}