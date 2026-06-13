using FluentAssertions;

using MergeGame.Application;
using MergeGame.Application.Commands;
using MergeGame.Application.Handlers;
using MergeGame.Application.Results;
using MergeGame.Domain;

using NSubstitute;

using Xunit;

namespace MergeGame.UnitTests.Application;

public sealed class MergeItemsHandlerTests
{
    [Fact]
    public void GivenTwoMatchingItems_WhenHandle_ThenReturnsSuccess()
    {
        MergeGrid grid = new MergeGrid(5, 5);
        GridPosition sourcePos = new GridPosition(0, 0);
        GridPosition targetPos = new GridPosition(1, 0);
        grid.PlaceItem(new MergeItem(new ItemLevel(1), sourcePos));
        grid.PlaceItem(new MergeItem(new ItemLevel(1), targetPos));

        MergeItemsHandler sut = new MergeItemsHandler(CreateSession(grid));

        MergeItemsResult result = sut.Handle(new MergeItemsCommand(sourcePos, targetPos));

        result.Should().BeOfType<MergeItemsResult.Success>()
            .Which.ProducedItem.Level.Value.Should().Be(2);
    }

    [Fact]
    public void GivenMismatchedLevels_WhenHandle_ThenReturnsFailed()
    {
        MergeGrid grid = new MergeGrid(5, 5);
        GridPosition sourcePos = new GridPosition(0, 0);
        GridPosition targetPos = new GridPosition(1, 0);
        grid.PlaceItem(new MergeItem(new ItemLevel(1), sourcePos));
        grid.PlaceItem(new MergeItem(new ItemLevel(2), targetPos));

        MergeItemsHandler sut = new MergeItemsHandler(CreateSession(grid));

        MergeItemsResult result = sut.Handle(new MergeItemsCommand(sourcePos, targetPos));

        result.Should().BeOfType<MergeItemsResult.Failed>();
    }

    [Fact]
    public void GivenEmptySourceCell_WhenHandle_ThenReturnsFailed()
    {
        MergeGrid grid = new MergeGrid(5, 5);
        GridPosition targetPos = new GridPosition(1, 0);
        grid.PlaceItem(new MergeItem(new ItemLevel(1), targetPos));

        MergeItemsHandler sut = new MergeItemsHandler(CreateSession(grid));

        MergeItemsResult result = sut.Handle(new MergeItemsCommand(new GridPosition(0, 0), targetPos));

        result.Should().BeOfType<MergeItemsResult.Failed>();
    }

    [Fact]
    public void GivenItemsAtMaxLevel_WhenHandle_ThenReturnsFailed()
    {
        MergeGrid grid = new MergeGrid(5, 5);
        GridPosition sourcePos = new GridPosition(0, 0);
        GridPosition targetPos = new GridPosition(1, 0);
        grid.PlaceItem(new MergeItem(new ItemLevel(10), sourcePos));
        grid.PlaceItem(new MergeItem(new ItemLevel(10), targetPos));

        MergeItemsHandler sut = new MergeItemsHandler(CreateSession(grid));

        MergeItemsResult result = sut.Handle(new MergeItemsCommand(sourcePos, targetPos));

        result.Should().BeOfType<MergeItemsResult.Failed>();
    }

    private static IGameSession CreateSession(MergeGrid grid)
    {
        IGameSession session = Substitute.For<IGameSession>();
        session.Grid.Returns(grid);
        session.MergeRule.Returns(new StandardMergeRule(maxLevel: 10));

        return session;
    }
}
