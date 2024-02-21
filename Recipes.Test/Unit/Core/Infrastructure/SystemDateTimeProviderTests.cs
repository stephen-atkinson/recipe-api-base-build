using FluentAssertions;
using Recipes.Core.Infrastructure;

namespace Recipes.Test.Unit.Core.Infrastructure;

[TestFixture]
public class SystemDateTimeProviderTests
{
    [Test]
    public void UtcNow_ReturnsCurrent()
    {
        // Arrange
        var systemDateTimeProvider = new SystemDateTimeProvider();
        var expected = DateTime.UtcNow;
        
        // Act
        var result = systemDateTimeProvider.UtcNow;
        
        // Assert
        result.Should().BeCloseTo(expected, TimeSpan.FromMilliseconds(1));
    }
}