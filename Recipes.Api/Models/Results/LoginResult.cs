namespace Recipes.Api.Models.Results;

public record LoginResult
{
    /// <summary>
    /// An access token for the endpoints that require authentication.
    /// </summary>
    /// <example>jeyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9</example>
    public string AccessToken { get; set; } = null!;
}