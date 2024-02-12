using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(200, Type = typeof(LoginResult))]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _recipesDbContext.Users.SingleOrDefaultAsync(u => u.Username == request.Username);

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
        public string Username { get; set; } = null!;
    }

    private record LoginResult
    {
        public string Token { get; set; } = null!;
        public string Scheme { get; set; } = null!;
    }
}