using Recipes.Api.Models.Results;
using Recipes.Core.Domain;

namespace Recipes.Api.Models.Requests.Groups;

public class CreateGroupRequest
{
    public string Name { get; set; } = null!;
    public Diet? Diet { get; set; }
    public Course? Course { get; set; }
    public ICollection<int> RecipeIds { get; set; } = null!;
}