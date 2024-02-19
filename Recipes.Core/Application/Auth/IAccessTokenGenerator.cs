using Recipes.Core.Domain;

namespace Recipes.Core.Application.Auth;

public interface IAccessTokenGenerator
{
    public string Create(ApplicationUser applicationUser);
}