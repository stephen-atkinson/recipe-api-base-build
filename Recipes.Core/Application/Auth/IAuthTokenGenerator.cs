using Recipes.Core.Domain;

namespace Recipes.Core.Application.Auth;

public interface IAuthTokenGenerator
{
    public string Create(User user);
}