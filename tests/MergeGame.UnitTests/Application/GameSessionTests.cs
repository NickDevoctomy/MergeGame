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
        // Arrange
        MergeGrid grid = new MergeGrid(5, 5);
        IMergeRule rule = Substitute.For<IMergeRule>();

        // Act
        GameSession sut = new GameSession(grid, rule);

        // Assert
        sut.Grid.Should().BeSameAs(grid);
    }

    [Fact]
    public void GivenValidArgs_WhenConstructed_ThenMergeRuleIsExposed()
    {
        // Arrange
        MergeGrid grid = new MergeGrid(5, 5);
        IMergeRule rule = Substitute.For<IMergeRule>();

        // Act
        GameSession sut = new GameSession(grid, rule);

        // Assert
        sut.MergeRule.Should().BeSameAs(rule);
    }

    [Fact]
    public void GivenNullGrid_WhenConstructed_ThenThrowsArgumentNullException()
    {
        // Arrange
        IMergeRule rule = Substitute.For<IMergeRule>();

        // Act
        Action act = () => _ = new GameSession(null!, rule);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenNullMergeRule_WhenConstructed_ThenThrowsArgumentNullException()
    {
        // Arrange
        MergeGrid grid = new MergeGrid(5, 5);

        // Act
        Action act = () => _ = new GameSession(grid, null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}
