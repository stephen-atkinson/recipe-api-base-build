namespace Recipes.Api.Models.Results;

public record LoginResult
{
    /// <summary>
    /// An access token for the endpoints that require authentication.
    /// </summary>
    /// <example>jeyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9</example>
    public string Token { get; set; } = null!;
        
    /// <summary>
    /// The scheme for the authorization header.
    /// </summary>
    /// <example>Bearer</example>
    public string Scheme { get; set; } = null!;
}