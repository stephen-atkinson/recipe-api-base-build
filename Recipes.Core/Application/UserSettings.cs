namespace Recipes.Core.Application;

public class UserSettings
{
    public IReadOnlyCollection<DefaultUser> DefaultUsers { get; set; } = null!;

    public class DefaultUser
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}