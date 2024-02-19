using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Recipes.Core.Application.Auth;

public class JwtSettings
{
    public string Audience { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string SigningKey { get; set; } = null!;

    public SymmetricSecurityKey GetSigningKey() =>
        new(Encoding.UTF8.GetBytes(SigningKey));
}