using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Recipes.Core.Application;

namespace Recipes.Api.Controllers;

[ApiController]
[Route("[controller]")]
[ApiVersionNeutral]
public class AuthController : ControllerBase
{
    public const string Issuer = "Recipes.Api";
    public const string Audience = Issuer;
    
    public static readonly SymmetricSecurityKey SigningKey = new (Encoding.UTF8.GetBytes("StoreThisSecurelyInARealWorldApplication"));
    
    private readonly IRecipesDbContext _recipesDbContext;

    public AuthController(IRecipesDbContext recipesDbContext)
    {
        _recipesDbContext = recipesDbContext;
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
        
        var user = await _recipesDbContext.Users
            .SingleOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

        if (user == null)
        {
            return Unauthorized();
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = Audience,
            Issuer = Issuer,
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(5),
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
            }),
            SigningCredentials = new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var result = new LoginResult
        {
            Token = tokenHandler.CreateEncodedJwt(tokenDescriptor),
            Scheme = JwtBearerDefaults.AuthenticationScheme
        };

        return Ok(result);
    }

    public record LoginRequest
    {
        /// <summary>
        /// The username of the user to authenticate as.
        /// </summary>
        /// <example>joe.bloggs</example>
        public string Username { get; set; } = null!;
    }

    private record LoginResult
    {
        /// <summary>
        /// An access token for the endpoints that require authentication.
        /// </summary>
        /// <example>jeyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9</example>
        public string Token { get; set; }
        
        /// <summary>
        /// The scheme for the authorization header.
        /// </summary>
        /// <example>Bearer</example>
        public string Scheme { get; set; } = null!;
    }
}