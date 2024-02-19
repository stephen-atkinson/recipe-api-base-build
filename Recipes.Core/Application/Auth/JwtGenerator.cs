using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Recipes.Core.Domain;

namespace Recipes.Core.Application.Auth;

public class JwtGenerator : IAccessTokenGenerator
{
    
    
    public const string Issuer = "Recipes";
    public const string Audience = Issuer;
    
    public static readonly SymmetricSecurityKey SigningKey = new (Encoding.UTF8.GetBytes("StoreThisSecurelyInARealWorldApplication"));
    
    public string Create(ApplicationUser applicationUser)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = Audience,
            Issuer = Issuer,
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(5),
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, applicationUser.Id),
            }),
            SigningCredentials = new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256Signature)
        };

        return tokenHandler.CreateEncodedJwt(tokenDescriptor);
    }
}