using System;

using FluentAssertions;

using MergeGame.Application;
using MergeGame.Domain;

using NSubstitute;

using Xunit;

namespace MergeGame.UnitTests.Application;

public sealed class GameSessionTests
{
    [Fact]
    public void GivenValidArgs_WhenConstructed_ThenGridIsExposed()
    {
        MergeGrid grid = new MergeGrid(5, 5);
        IMergeRule rule = Substitute.For<IMergeRule>();

        GameSession sut = new GameSession(grid, rule);

        sut.Grid.Should().BeSameAs(grid);
    }

    [Fact]
    public void GivenValidArgs_WhenConstructed_ThenMergeRuleIsExposed()
    {
        MergeGrid grid = new MergeGrid(5, 5);
        IMergeRule rule = Substitute.For<IMergeRule>();

        GameSession sut = new GameSession(grid, rule);

        sut.MergeRule.Should().BeSameAs(rule);
    }

    [Fact]
    public void GivenNullGrid_WhenConstructed_ThenThrowsArgumentNullException()
    {
        IMergeRule rule = Substitute.For<IMergeRule>();

        Action act = () => _ = new GameSession(null!, rule);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenNullMergeRule_WhenConstructed_ThenThrowsArgumentNullException()
    {
        MergeGrid grid = new MergeGrid(5, 5);

        Action act = () => _ = new GameSession(grid, null!);

        act.Should().Throw<ArgumentNullException>();
    }
}
