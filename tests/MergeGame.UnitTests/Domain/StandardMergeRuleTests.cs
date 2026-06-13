using FluentAssertions;

using MergeGame.Domain;

using Xunit;

namespace MergeGame.UnitTests.Domain;

public sealed class StandardMergeRuleTests
{
    private readonly StandardMergeRule _sut = new StandardMergeRule(maxLevel: 10);

    [Fact]
    public void GivenTwoItemsOfSameLevel_WhenCanMerge_ThenReturnsTrue()
    {
        MergeItem a = new MergeItem(new ItemLevel(1), new GridPosition(0, 0));
        MergeItem b = new MergeItem(new ItemLevel(1), new GridPosition(1, 0));

        bool result = _sut.CanMerge(a, b);

        result.Should().BeTrue();
    }

    [Fact]
    public void GivenTwoItemsOfDifferentLevel_WhenCanMerge_ThenReturnsFalse()
    {
        MergeItem a = new MergeItem(new ItemLevel(1), new GridPosition(0, 0));
        MergeItem b = new MergeItem(new ItemLevel(2), new GridPosition(1, 0));

        bool result = _sut.CanMerge(a, b);

        result.Should().BeFalse();
    }

    [Fact]
    public void GivenBothItemsAtMaxLevel_WhenCanMerge_ThenReturnsFalse()
    {
        MergeItem a = new MergeItem(new ItemLevel(10), new GridPosition(0, 0));
        MergeItem b = new MergeItem(new ItemLevel(10), new GridPosition(1, 0));

        bool result = _sut.CanMerge(a, b);

        result.Should().BeFalse();
    }

    [Fact]
    public void GivenTwoMergeableItems_WhenMerge_ThenProducesItemAtNextLevel()
    {
        MergeItem a = new MergeItem(new ItemLevel(3), new GridPosition(0, 0));
        MergeItem b = new MergeItem(new ItemLevel(3), new GridPosition(1, 0));
        GridPosition target = new GridPosition(1, 0);

        MergeItem result = _sut.Merge(a, b, target);

        result.Level.Value.Should().Be(4);
        result.Position.Should().Be(target);
    }

    [Fact]
    public void GivenNonMergeableItems_WhenMerge_ThenThrowsInvalidOperationException()
    {
        MergeItem a = new MergeItem(new ItemLevel(1), new GridPosition(0, 0));
        MergeItem b = new MergeItem(new ItemLevel(2), new GridPosition(1, 0));

        Action act = () => _sut.Merge(a, b, new GridPosition(1, 0));

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GivenZeroMaxLevel_WhenConstructed_ThenThrowsArgumentOutOfRangeException()
    {
        Action act = () => _ = new StandardMergeRule(maxLevel: 0);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
