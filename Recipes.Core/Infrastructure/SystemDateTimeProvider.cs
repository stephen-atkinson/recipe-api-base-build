using Recipes.Core.Application;
using Recipes.Core.Application.Contracts;

namespace Recipes.Core.Infrastructure;

public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}