using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recipes.Api.Models.Dtos;
using Recipes.Api.Models.Requests;
using Recipes.Core.Application.Auth;

namespace Recipes.Api.Controllers;

[ApiController]
[Route("[controller]")]
[ApiVersionNeutral]
public class AuthController : ControllerBase
{
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly AspNetUserManager<IdentityUser> _aspNetUserManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AuthController(
        IAccessTokenGenerator accessTokenGenerator,
        AspNetUserManager<IdentityUser> aspNetUserManager,
        SignInManager<IdentityUser> signInManager)
    {
        _accessTokenGenerator = accessTokenGenerator;
        _aspNetUserManager = aspNetUserManager;
        _signInManager = signInManager;
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

        var user = await _aspNetUserManager.FindByNameAsync(request.Username);

        if (user == null)
        {
            return Unauthorized();
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!signInResult.Succeeded)
        {
            return Unauthorized();
        }

        var accessToken = _accessTokenGenerator.Create(user);

        var result = new LoginResult
        {
            AccessToken = accessToken
        };

        return Ok(result);
    }
}