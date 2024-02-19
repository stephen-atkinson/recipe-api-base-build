using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Recipes.Core.Domain;

namespace Recipes.Core.Application.Auth;

public class JwtGenerator : IAccessTokenGenerator
{
    private readonly IOptionsMonitor<JwtSettings> _jwtOptions;

    public JwtGenerator(IOptionsMonitor<JwtSettings> jwtOptions)
    {
        _jwtOptions = jwtOptions;
    }
    
    public string Create(ApplicationUser applicationUser)
    {
        var jwtSettings = _jwtOptions.CurrentValue;
        
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = jwtSettings.Audience,
            Issuer = jwtSettings.Issuer,
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(5),
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, applicationUser.Id),
            }),
            SigningCredentials = new SigningCredentials(jwtSettings.GetSigningKey(), SecurityAlgorithms.HmacSha256Signature)
        };

        return tokenHandler.CreateEncodedJwt(tokenDescriptor);
    }
}