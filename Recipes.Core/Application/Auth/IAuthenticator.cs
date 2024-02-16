using Recipes.Core.Domain;

namespace Recipes.Core.Application.Auth;

public interface IAuthenticator
{
    Task<User?> ByUsernameAsync(string username, CancellationToken cancellationToken);
}