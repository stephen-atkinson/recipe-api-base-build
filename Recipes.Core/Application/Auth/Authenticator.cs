using Microsoft.EntityFrameworkCore;
using Recipes.Core.Domain;

namespace Recipes.Core.Application.Auth;

public class Authenticator : IAuthenticator
{
    private readonly IRecipesDbContext _recipesDbContext;

    public Authenticator(IRecipesDbContext recipesDbContext)
    {
        _recipesDbContext = recipesDbContext;
    }
    
    public async Task<User?> ByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(username);
        
        var user = await _recipesDbContext.Users
            .SingleOrDefaultAsync(u => u.Username == username, cancellationToken);

        return user;
    }
}