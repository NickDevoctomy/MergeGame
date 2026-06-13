using FluentAssertions;

using MergeGame.Infrastructure.Random;

using Xunit;

namespace MergeGame.UnitTests.Infrastructure;

public sealed class SystemRandomProviderTests
{
    [Fact]
    public void GivenRange_WhenGetNext_ThenReturnedValueIsWithinRange()
    {
        SystemRandomProvider sut = new SystemRandomProvider();

        int result = sut.GetNext(0, 10);

        result.Should().BeGreaterThanOrEqualTo(0).And.BeLessThan(10);
    }

    [Fact]
    public void GivenSingleValueRange_WhenGetNext_ThenReturnsMinInclusive()
    {
        SystemRandomProvider sut = new SystemRandomProvider();

        int result = sut.GetNext(5, 6);

        result.Should().Be(5);
    }
}
