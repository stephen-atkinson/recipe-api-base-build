namespace Recipes.Core.Application;

public class UserSettings
{
    public IReadOnlyCollection<string> DefaultUsers { get; set; } = null!;
}