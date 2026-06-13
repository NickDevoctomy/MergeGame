using FluentAssertions;

using MergeGame.Infrastructure.Random;

using Xunit;

namespace MergeGame.UnitTests.Infrastructure;

public sealed class SystemRandomProviderTests
{
    [Fact]
    public void GivenRange_WhenGetNext_ThenReturnedValueIsWithinRange()
    {
        // Arrange
        SystemRandomProvider sut = new SystemRandomProvider();

        // Act
        int result = sut.GetNext(0, 10);

        // Assert
        result.Should().BeGreaterThanOrEqualTo(0).And.BeLessThan(10);
    }

    [Fact]
    public void GivenSingleValueRange_WhenGetNext_ThenReturnsMinInclusive()
    {
        // Arrange
        SystemRandomProvider sut = new SystemRandomProvider();

        // Act
        int result = sut.GetNext(5, 6);

        // Assert
        result.Should().Be(5);
    }
}
