using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Recipes.Core.Application.Models;
using Recipes.Core.Infrastructure.Database;

namespace Recipes.Core.Application.Extensions;

public static class HostExtensions
{
    public static async Task EnsureDatabase(this IHost host)
    {
        await using var scope = host.Services.CreateAsyncScope();
        
        var dbContext = scope.ServiceProvider.GetRequiredService<RecipesDbContext>();
        
        // Don't do this in a real-world application. This is just to simplify database creation if you want to start over in the exercises.
        // https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli
        await dbContext.Database.MigrateAsync();
    }
    
    public static async Task SeedUsers(this IHost host)
    {
        await using var scope = host.Services.CreateAsyncScope();
        
        var aspNetUserManager = scope.ServiceProvider.GetRequiredService<AspNetUserManager<IdentityUser>>();
        
        var userOptions = scope.ServiceProvider.GetRequiredService<IOptions<UserSettings>>();

        foreach (var defaultUser in userOptions.Value.DefaultUsers)
        {
            var user = await aspNetUserManager.FindByNameAsync(defaultUser.Username);

            if (user != null)
            {
                continue;
            }
            
            user = new IdentityUser { UserName = defaultUser.Username };

            var result = await aspNetUserManager.CreateAsync(user, defaultUser.Password);
            
            if (!result.Succeeded)
            {
                throw new Exception("Seed users failed");
            }
        }
    }
}