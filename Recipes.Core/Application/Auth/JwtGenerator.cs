using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Recipes.Core.Domain;

namespace Recipes.Core.Application.Auth;

public class JwtGenerator : IAccessTokenGenerator
{
    private readonly IOptionsMonitor<JwtSettings> _jwtOptions;
    private readonly IDateTimeProvider _dateTimeProvider;

    public JwtGenerator(IOptionsMonitor<JwtSettings> jwtOptions, IDateTimeProvider dateTimeProvider)
    {
        _jwtOptions = jwtOptions;
        _dateTimeProvider = dateTimeProvider;
    }
    
    public string Create(ApplicationUser applicationUser)
    {
        var jwtSettings = _jwtOptions.CurrentValue;

        var utcNow = _dateTimeProvider.UtcNow;
        
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = jwtSettings.Audience,
            Issuer = jwtSettings.Issuer,
            IssuedAt = utcNow,
            Expires = utcNow.Add(jwtSettings.ExpiresIn),
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, applicationUser.Id),
            }),
            SigningCredentials = new SigningCredentials(jwtSettings.GetSigningKey(), SecurityAlgorithms.HmacSha256Signature)
        };

        return tokenHandler.CreateEncodedJwt(tokenDescriptor);
    }
}