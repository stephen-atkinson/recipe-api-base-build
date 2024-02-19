namespace Recipes.Core.Application;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}