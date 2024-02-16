namespace Recipes.Api.Models.Requests;

public record LoginRequest
{
    /// <summary>
    /// The username of the user to authenticate as.
    /// </summary>
    /// <example>joe.bloggs</example>
    public string Username { get; set; } = null!;
}