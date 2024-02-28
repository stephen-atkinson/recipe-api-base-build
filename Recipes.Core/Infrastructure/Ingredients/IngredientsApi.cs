using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Recipes.Core.Application.Contracts;
using Recipes.Core.Application.Models;

namespace Recipes.Core.Infrastructure.Ingredients;

public class IngredientsApi : IIngredientsApi
{
    private readonly IOptions<IngredientsApiSettings> _options;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public IngredientsApi(IOptions<IngredientsApiSettings> options, HttpClient httpClient)
    {
        _options = options;
        _httpClient = httpClient;
        _jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    }

    public async Task<bool> IsHealthyAsync(CancellationToken cancellationToken)
    {
        var host = new Uri(_options.Value.BaseUrl).Host;
        
        using var ping = new Ping();

        // Ping cancellation added in .NET 8.
        var replyTask = await ping.SendPingAsync(host);

        return replyTask.Status == IPStatus.Success;
    }

    public Task<IReadOnlyCollection<ExternalIngredient>> BatchGet(BatchGetIngredientsRequest request, CancellationToken cancellationToken) =>
        SendAsync<IReadOnlyCollection<ExternalIngredient>>(HttpMethod.Post, "/api/ingredients/batch-read", request, cancellationToken);


    private async Task<T> SendAsync<T>(HttpMethod method, string path, object? body, CancellationToken cancellationToken)
    {
        var responseContent = await SendAsync(method, path, body, cancellationToken);

        var responseT = JsonSerializer.Deserialize<T>(responseContent, _jsonSerializerOptions);

        return responseT;
    }

    private async Task<string> SendAsync(HttpMethod method, string path, object? body, CancellationToken cancellationToken)
    {
        var uri = new UriBuilder(_options.Value.BaseUrl)
        {
            Path = path
        }.Uri;

        var requestMessage = new HttpRequestMessage(method, uri);
        
        requestMessage.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json; x-api-version=2.0"));

        if (body != null)
        {
            requestMessage.Content = JsonContent.Create(body, null, _jsonSerializerOptions);
        }

        var response = await _httpClient.SendAsync(requestMessage, cancellationToken);

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Ingredients API request failed. Url: {uri}. Response: {responseContent}", null, response.StatusCode);
        }

        return responseContent;
    }
}