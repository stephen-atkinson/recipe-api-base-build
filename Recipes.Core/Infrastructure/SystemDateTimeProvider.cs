using Recipes.Core.Application;

namespace Recipes.Core.Infrastructure;

public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}