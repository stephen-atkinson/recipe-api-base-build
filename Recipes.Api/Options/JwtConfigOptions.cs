using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Recipes.Core.Application.Auth;

namespace Recipes.Api.Options;

public class JwtConfigOptions : IConfigureOptions<JwtBearerOptions>
{
    private readonly IOptionsMonitor<JwtSettings> _settingsMonitor;

    public JwtConfigOptions(IOptionsMonitor<JwtSettings> settingsMonitor)
    {
        _settingsMonitor = settingsMonitor;
    }
    
    public void Configure(JwtBearerOptions options)
    {
        var settings = _settingsMonitor.CurrentValue;

        options.TokenValidationParameters.IssuerSigningKey = JwtGenerator.SigningKey;
        options.TokenValidationParameters.ValidIssuer = settings.Issuer;
        options.TokenValidationParameters.ValidAudience = settings.Audience;
    }
}