using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recipes.Api.Models.Requests;
using Recipes.Api.Models.Results;
using Recipes.Core.Application.Auth;

namespace Recipes.Api.Controllers;

[ApiController]
[Route("[controller]")]
[ApiVersionNeutral]
public class AuthController : ControllerBase
{
    private readonly IAuthenticator _authenticator;
    private readonly IAuthTokenGenerator _authTokenGenerator;

    public AuthController(
        IAuthenticator authenticator,
        IAuthTokenGenerator authTokenGenerator)
    {
        _authenticator = authenticator;
        _authTokenGenerator = authTokenGenerator;
    }

    /// <summary>
    /// Authenticate a user.
    /// </summary>
    /// <remarks>Authenticates the user using their username as the credentials.</remarks>
    /// <param name="request">The request's body.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe.</param>
    /// <returns>A <see cref="LoginResult"/> that contains an access token.</returns>
    /// <response code="200">The user has successfully authenticated.</response>
    /// <response code="400">The request is invalid.</response>
    /// <response code="401">The user couldn't be authenticated.</response>
    /// <response code="500">Oops! Something went wrong.</response>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(200, Type = typeof(LoginResult))]
    [ProducesResponseType(400, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(401, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Index(LoginRequest request, CancellationToken cancellationToken)
    {
        // Request validation removed from base build.

        var user = await _authenticator.ByUsernameAsync(request.Username, cancellationToken);

        if (user == null)
        {
            return Unauthorized();
        }

        var token = _authTokenGenerator.Create(user);

        var result = new LoginResult
        {
            Token = token,
            Scheme = "Bearer"
        };

        return Ok(result);
    }
}