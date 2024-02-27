namespace Recipes.Api.Versions.Neutral.Models.Requests;

public record LoginRequest
{
    /// <summary>
    /// The username of the user to authenticate as.
    /// </summary>
    /// <example>joe.bloggs</example>
    public string Username { get; set; } = null!;

    /// <summary>
    /// The password of the user to authenticate as.
    /// </summary>
    /// <example>Joe123!</example>
    public string Password { get; set; } = null!;
}