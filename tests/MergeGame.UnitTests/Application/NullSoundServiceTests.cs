using FluentAssertions;

using MergeGame.Application;

using Xunit;

namespace MergeGame.UnitTests.Application;

public sealed class NullSoundServiceTests
{
    [Fact]
    public void GivenAnyKey_WhenPlaySound_ThenDoesNotThrow()
    {
        NullSoundService sut = new NullSoundService();

        Action act = () => sut.PlaySound("merge");

        act.Should().NotThrow();
    }

    [Fact]
    public void GivenEmptyKey_WhenPlaySound_ThenDoesNotThrow()
    {
        NullSoundService sut = new NullSoundService();

        Action act = () => sut.PlaySound(string.Empty);

        act.Should().NotThrow();
    }
}
