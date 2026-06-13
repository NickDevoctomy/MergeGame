using FluentAssertions;

using MergeGame.Application;

using Xunit;

namespace MergeGame.UnitTests.Application;

public sealed class NullSoundServiceTests
{
    [Fact]
    public void GivenAnyKey_WhenPlaySound_ThenDoesNotThrow()
    {
        // Arrange
        NullSoundService sut = new NullSoundService();

        // Act
        Action act = () => sut.PlaySound("merge");

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void GivenEmptyKey_WhenPlaySound_ThenDoesNotThrow()
    {
        // Arrange
        NullSoundService sut = new NullSoundService();

        // Act
        Action act = () => sut.PlaySound(string.Empty);

        // Assert
        act.Should().NotThrow();
    }
}
