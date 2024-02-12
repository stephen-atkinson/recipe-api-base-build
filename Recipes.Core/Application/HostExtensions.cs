using Microsoft.EntityFrameworkCore;
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
        
        var dbContext = scope.ServiceProvider.GetRequiredService<IRecipesDbContext>();
        var userOptions = scope.ServiceProvider.GetRequiredService<IOptions<UserSettings>>();

        var users = await dbContext.Users
            .Where(u => userOptions.Value.DefaultUsers.Contains(u.Username))
            .ToArrayAsync();

        foreach (var username in userOptions.Value.DefaultUsers)
        {
            var user = users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));

            if (user != null)
            {
                continue;
            }

            user = new User
            {
                Username = username
            };

            await dbContext.Users.AddAsync(user);
        }

        await dbContext.SaveChangesAsync();
    }
}