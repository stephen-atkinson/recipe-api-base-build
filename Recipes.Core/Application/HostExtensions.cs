using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Recipes.Core.Domain;

namespace Recipes.Core.Application;

public static class HostExtensions
{
    public static async Task SeedUsers(this IHost host)
    {
        await using var scope = host.Services.CreateAsyncScope();

        var aspNetUserManager = scope.ServiceProvider.GetRequiredService<AspNetUserManager<ApplicationUser>>();
        
        var userOptions = scope.ServiceProvider.GetRequiredService<IOptions<UserSettings>>();

        foreach (var defaultUser in userOptions.Value.DefaultUsers)
        {
            var user = new ApplicationUser { UserName = defaultUser.Username };

            await aspNetUserManager.CreateAsync(user, defaultUser.Password);
        }
    }
}