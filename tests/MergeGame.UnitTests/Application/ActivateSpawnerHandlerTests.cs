using System.Collections.Generic;

using FluentAssertions;

using MergeGame.Application;
using MergeGame.Application.Commands;
using MergeGame.Application.Handlers;
using MergeGame.Application.Results;
using MergeGame.Domain;

using NSubstitute;

using Xunit;

namespace MergeGame.UnitTests.Application;

public sealed class ActivateSpawnerHandlerTests
{
    [Fact]
    public void GivenSpawnerCell_AndEmptyGridSpaces_WhenHandle_ThenReturnsSuccessWithPlacedItem()
    {
        MergeGrid grid = new MergeGrid(3, 3);
        GridPosition spawnerPos = new GridPosition(0, 0);
        grid.PlaceSpawner(spawnerPos, CreateDefinition());

        IRandomProvider random = Substitute.For<IRandomProvider>();
        random.GetNext(Arg.Any<int>(), Arg.Any<int>()).Returns(0);

        IGameSession session = Substitute.For<IGameSession>();
        session.Grid.Returns(grid);

        ActivateSpawnerHandler sut = new ActivateSpawnerHandler(session, new SpawnerService(), random);

        ActivateSpawnerResult result = sut.Handle(new ActivateSpawnerCommand(spawnerPos));

        result.Should().BeOfType<ActivateSpawnerResult.Success>();
    }

    [Fact]
    public void GivenNonSpawnerCell_WhenHandle_ThenReturnsNotASpawner()
    {
        MergeGrid grid = new MergeGrid(3, 3);
        GridPosition pos = new GridPosition(1, 1);

        IRandomProvider random = Substitute.For<IRandomProvider>();
        IGameSession session = Substitute.For<IGameSession>();
        session.Grid.Returns(grid);

        ActivateSpawnerHandler sut = new ActivateSpawnerHandler(session, new SpawnerService(), random);

        ActivateSpawnerResult result = sut.Handle(new ActivateSpawnerCommand(pos));

        result.Should().BeOfType<ActivateSpawnerResult.NotASpawner>();
    }

    [Fact]
    public void GivenSpawnerCell_AndGridIsFull_WhenHandle_ThenReturnsGridFull()
    {
        MergeGrid grid = new MergeGrid(1, 1);
        GridPosition spawnerPos = new GridPosition(0, 0);
        grid.PlaceSpawner(spawnerPos, CreateDefinition());

        IRandomProvider random = Substitute.For<IRandomProvider>();
        IGameSession session = Substitute.For<IGameSession>();
        session.Grid.Returns(grid);

        ActivateSpawnerHandler sut = new ActivateSpawnerHandler(session, new SpawnerService(), random);

        ActivateSpawnerResult result = sut.Handle(new ActivateSpawnerCommand(spawnerPos));

        result.Should().BeOfType<ActivateSpawnerResult.GridFull>();
    }

    [Fact]
    public void GivenSpawnerActivated_WhenHandle_ThenNewItemAppearsInEmptyCell()
    {
        MergeGrid grid = new MergeGrid(2, 1);
        GridPosition spawnerPos = new GridPosition(0, 0);
        grid.PlaceSpawner(spawnerPos, CreateDefinition());

        IRandomProvider random = Substitute.For<IRandomProvider>();
        random.GetNext(Arg.Any<int>(), Arg.Any<int>()).Returns(0);

        IGameSession session = Substitute.For<IGameSession>();
        session.Grid.Returns(grid);

        ActivateSpawnerHandler sut = new ActivateSpawnerHandler(session, new SpawnerService(), random);

        sut.Handle(new ActivateSpawnerCommand(spawnerPos));

        grid.FindEmptyCells().Should().HaveCount(0);
    }

    private static SpawnerDefinition CreateDefinition()
    {
        ItemDefinition def = new ItemDefinition("wood", string.Empty, "wood.png", null);

        return new SpawnerDefinition(new Dictionary<ItemDefinition, int> { [def] = 100 });
    }
}
