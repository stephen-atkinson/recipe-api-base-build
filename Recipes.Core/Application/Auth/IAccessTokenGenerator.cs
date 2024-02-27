using Microsoft.AspNetCore.Identity;
using Recipes.Core.Domain;

namespace Recipes.Core.Application.Auth;

public interface IAccessTokenGenerator
{
    public string Create(IdentityUser user);
}