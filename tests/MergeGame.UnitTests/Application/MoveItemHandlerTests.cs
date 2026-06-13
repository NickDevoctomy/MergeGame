using FluentAssertions;

using MergeGame.Application;
using MergeGame.Application.Commands;
using MergeGame.Application.Handlers;
using MergeGame.Application.Results;
using MergeGame.Domain;

using NSubstitute;

using Xunit;

namespace MergeGame.UnitTests.Application;

public sealed class MoveItemHandlerTests
{
    [Fact]
    public void GivenItemAtSource_AndEmptyTarget_WhenHandle_ThenReturnsSuccess()
    {
        MergeGrid grid = new MergeGrid(5, 5);
        GridPosition sourcePos = new GridPosition(0, 0);
        GridPosition targetPos = new GridPosition(2, 2);
        grid.PlaceItem(new MergeItem(new ItemLevel(1), sourcePos));

        MoveItemHandler sut = new MoveItemHandler(CreateSession(grid));

        MoveItemResult result = sut.Handle(new MoveItemCommand(sourcePos, targetPos));

        result.Should().BeOfType<MoveItemResult.Success>();
    }

    [Fact]
    public void GivenItemAtSource_AndEmptyTarget_WhenHandle_ThenItemAppearsAtTarget()
    {
        MergeGrid grid = new MergeGrid(5, 5);
        GridPosition sourcePos = new GridPosition(0, 0);
        GridPosition targetPos = new GridPosition(2, 2);
        grid.PlaceItem(new MergeItem(new ItemLevel(3), sourcePos));

        MoveItemHandler sut = new MoveItemHandler(CreateSession(grid));
        sut.Handle(new MoveItemCommand(sourcePos, targetPos));

        grid.GetCell(targetPos).Should().BeOfType<CellContent.Item>()
            .Which.MergeItem.Level.Value.Should().Be(3);
        grid.GetCell(sourcePos).Should().Be(CellContent.Empty.Instance);
    }

    [Fact]
    public void GivenEmptySource_WhenHandle_ThenReturnsFailed()
    {
        MergeGrid grid = new MergeGrid(5, 5);

        MoveItemHandler sut = new MoveItemHandler(CreateSession(grid));

        MoveItemResult result = sut.Handle(new MoveItemCommand(new GridPosition(0, 0), new GridPosition(1, 1)));

        result.Should().BeOfType<MoveItemResult.Failed>();
    }

    [Fact]
    public void GivenItemAtSource_AndOccupiedTarget_WhenHandle_ThenReturnsFailed()
    {
        MergeGrid grid = new MergeGrid(5, 5);
        GridPosition sourcePos = new GridPosition(0, 0);
        GridPosition targetPos = new GridPosition(1, 0);
        grid.PlaceItem(new MergeItem(new ItemLevel(1), sourcePos));
        grid.PlaceItem(new MergeItem(new ItemLevel(2), targetPos));

        MoveItemHandler sut = new MoveItemHandler(CreateSession(grid));

        MoveItemResult result = sut.Handle(new MoveItemCommand(sourcePos, targetPos));

        result.Should().BeOfType<MoveItemResult.Failed>();
    }

    private static IGameSession CreateSession(MergeGrid grid)
    {
        IGameSession session = Substitute.For<IGameSession>();
        session.Grid.Returns(grid);
        session.MergeRule.Returns(new StandardMergeRule(maxLevel: 10));

        return session;
    }
}
