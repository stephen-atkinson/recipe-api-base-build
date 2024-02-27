namespace Recipes.Core.Application.Contracts;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}