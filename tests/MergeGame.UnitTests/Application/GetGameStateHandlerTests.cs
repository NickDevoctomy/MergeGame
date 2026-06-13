using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using MergeGame.Application;
using MergeGame.Application.Dtos;
using MergeGame.Application.Handlers;
using MergeGame.Application.Queries;
using MergeGame.Domain;

using NSubstitute;

using Xunit;

namespace MergeGame.UnitTests.Application;

public sealed class GetGameStateHandlerTests
{
    [Fact]
    public void GivenEmptyGrid_WhenHandle_ThenSnapshotHasAllEmptyCells()
    {
        MergeGrid grid = new MergeGrid(2, 2);
        IGameSession session = Substitute.For<IGameSession>();
        session.Grid.Returns(grid);

        GetGameStateHandler sut = new GetGameStateHandler(session);

        GameStateSnapshot result = sut.Handle(new GetGameStateQuery());

        result.Cells.Should().HaveCount(4);
        result.Cells.Should().AllSatisfy(c => c.IsEmpty.Should().BeTrue());
    }

    [Fact]
    public void GivenItemOnGrid_WhenHandle_ThenSnapshotReflectsItemLevel()
    {
        MergeGrid grid = new MergeGrid(3, 3);
        GridPosition itemPos = new GridPosition(1, 1);
        grid.PlaceItem(new MergeItem(new ItemLevel(5), itemPos));

        IGameSession session = Substitute.For<IGameSession>();
        session.Grid.Returns(grid);

        GetGameStateHandler sut = new GetGameStateHandler(session);

        GameStateSnapshot result = sut.Handle(new GetGameStateQuery());

        CellStateDto itemCell = result.Cells.Single(c => c.Position == itemPos);
        itemCell.Level.Should().Be(5);
        itemCell.IsEmpty.Should().BeFalse();
        itemCell.IsSpawner.Should().BeFalse();
    }

    [Fact]
    public void GivenSpawnerOnGrid_WhenHandle_ThenSnapshotReflectsSpawner()
    {
        MergeGrid grid = new MergeGrid(3, 3);
        GridPosition spawnerPos = new GridPosition(0, 0);
        SpawnerDefinition def = new SpawnerDefinition(
            new Dictionary<ItemLevel, int> { [new ItemLevel(1)] = 1 });
        grid.PlaceSpawner(spawnerPos, def);

        IGameSession session = Substitute.For<IGameSession>();
        session.Grid.Returns(grid);

        GetGameStateHandler sut = new GetGameStateHandler(session);

        GameStateSnapshot result = sut.Handle(new GetGameStateQuery());

        CellStateDto spawnerCell = result.Cells.Single(c => c.Position == spawnerPos);
        spawnerCell.IsSpawner.Should().BeTrue();
        spawnerCell.IsEmpty.Should().BeFalse();
        spawnerCell.Level.Should().BeNull();
    }

    [Fact]
    public void GivenGrid_WhenHandle_ThenSnapshotDimensionsMatchGrid()
    {
        MergeGrid grid = new MergeGrid(4, 6);
        IGameSession session = Substitute.For<IGameSession>();
        session.Grid.Returns(grid);

        GetGameStateHandler sut = new GetGameStateHandler(session);

        GameStateSnapshot result = sut.Handle(new GetGameStateQuery());

        result.Columns.Should().Be(4);
        result.Rows.Should().Be(6);
        result.Cells.Should().HaveCount(24);
    }
}
