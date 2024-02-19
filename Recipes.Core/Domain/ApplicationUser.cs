using Microsoft.AspNetCore.Identity;

namespace Recipes.Core.Domain;

public class ApplicationUser : IdentityUser
{
    public ICollection<Recipe> Recipes { get; set; } = null!;
}