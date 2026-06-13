using System.Collections.Generic;

using FluentAssertions;

using MergeGame.Domain;

using Xunit;

namespace MergeGame.UnitTests.Domain;

public sealed class MergeGridTests
{
    private readonly StandardMergeRule _rule = new StandardMergeRule(maxLevel: 10);

    [Fact]
    public void GivenNewGrid_WhenGetCell_ThenAllCellsAreEmpty()
    {
        MergeGrid sut = new MergeGrid(3, 3);

        CellContent cell = sut.GetCell(new GridPosition(1, 1));

        cell.Should().Be(CellContent.Empty.Instance);
    }

    [Fact]
    public void GivenEmptyCell_WhenPlaceItem_ThenCellContainsItem()
    {
        MergeGrid sut = new MergeGrid(5, 5);
        MergeItem item = new MergeItem(new ItemLevel(1), new GridPosition(2, 2));

        sut.PlaceItem(item);

        CellContent cell = sut.GetCell(new GridPosition(2, 2));
        cell.Should().BeOfType<CellContent.Item>()
            .Which.MergeItem.Level.Value.Should().Be(1);
    }

    [Fact]
    public void GivenOccupiedCell_WhenPlaceItem_ThenThrowsInvalidOperationException()
    {
        MergeGrid sut = new MergeGrid(5, 5);
        GridPosition pos = new GridPosition(0, 0);
        sut.PlaceItem(new MergeItem(new ItemLevel(1), pos));

        Action act = () => sut.PlaceItem(new MergeItem(new ItemLevel(2), pos));

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GivenOutOfBoundsPosition_WhenGetCell_ThenThrowsArgumentOutOfRangeException()
    {
        MergeGrid sut = new MergeGrid(3, 3);

        Action act = () => sut.GetCell(new GridPosition(5, 5));

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void GivenTwoMatchingItems_WhenTryMerge_ThenReturnsSuccessAndUpdatesGrid()
    {
        MergeGrid sut = new MergeGrid(5, 5);
        GridPosition sourcePos = new GridPosition(0, 0);
        GridPosition targetPos = new GridPosition(1, 0);
        sut.PlaceItem(new MergeItem(new ItemLevel(2), sourcePos));
        sut.PlaceItem(new MergeItem(new ItemLevel(2), targetPos));

        MergeResult result = sut.TryMerge(sourcePos, targetPos, _rule);

        result.Should().BeOfType<MergeResult.Success>()
            .Which.ProducedItem.Level.Value.Should().Be(3);
        sut.GetCell(sourcePos).Should().Be(CellContent.Empty.Instance);
        sut.GetCell(targetPos).Should().BeOfType<CellContent.Item>()
            .Which.MergeItem.Level.Value.Should().Be(3);
    }

    [Fact]
    public void GivenMismatchedItems_WhenTryMerge_ThenReturnsFailure()
    {
        MergeGrid sut = new MergeGrid(5, 5);
        GridPosition sourcePos = new GridPosition(0, 0);
        GridPosition targetPos = new GridPosition(1, 0);
        sut.PlaceItem(new MergeItem(new ItemLevel(1), sourcePos));
        sut.PlaceItem(new MergeItem(new ItemLevel(2), targetPos));

        MergeResult result = sut.TryMerge(sourcePos, targetPos, _rule);

        result.Should().BeOfType<MergeResult.Failure>();
    }

    [Fact]
    public void GivenEmptySourceCell_WhenTryMerge_ThenReturnsFailure()
    {
        MergeGrid sut = new MergeGrid(5, 5);
        GridPosition targetPos = new GridPosition(1, 0);
        sut.PlaceItem(new MergeItem(new ItemLevel(1), targetPos));

        MergeResult result = sut.TryMerge(new GridPosition(0, 0), targetPos, _rule);

        result.Should().BeOfType<MergeResult.Failure>();
    }

    [Fact]
    public void GivenPartiallyFilledGrid_WhenFindEmptyCells_ThenReturnsOnlyEmptyCells()
    {
        MergeGrid sut = new MergeGrid(2, 2);
        sut.PlaceItem(new MergeItem(new ItemLevel(1), new GridPosition(0, 0)));

        IReadOnlyList<GridPosition> empty = sut.FindEmptyCells();

        empty.Should().HaveCount(3);
        empty.Should().NotContain(new GridPosition(0, 0));
    }

    [Fact]
    public void GivenEmptyCell_WhenPlaceSpawner_ThenCellContainsSpawner()
    {
        MergeGrid sut = new MergeGrid(5, 5);
        GridPosition pos = new GridPosition(0, 0);
        SpawnerDefinition def = new SpawnerDefinition(
            new Dictionary<ItemLevel, int> { [new ItemLevel(1)] = 1 });

        sut.PlaceSpawner(pos, def);

        sut.GetCell(pos).Should().BeOfType<CellContent.Spawner>();
    }

    [Fact]
    public void GivenSpawnerCell_WhenFindEmptyCells_ThenSpawnerCellIsNotIncluded()
    {
        MergeGrid sut = new MergeGrid(2, 2);
        GridPosition spawnerPos = new GridPosition(0, 0);
        SpawnerDefinition def = new SpawnerDefinition(
            new Dictionary<ItemLevel, int> { [new ItemLevel(1)] = 1 });
        sut.PlaceSpawner(spawnerPos, def);

        IReadOnlyList<GridPosition> empty = sut.FindEmptyCells();

        empty.Should().HaveCount(3);
        empty.Should().NotContain(spawnerPos);
    }

    [Fact]
    public void GivenItemAtSource_AndEmptyTarget_WhenMoveItem_ThenReturnsSuccess()
    {
        MergeGrid sut = new MergeGrid(5, 5);
        GridPosition source = new GridPosition(0, 0);
        GridPosition target = new GridPosition(3, 3);
        sut.PlaceItem(new MergeItem(new ItemLevel(2), source));

        MoveResult result = sut.MoveItem(source, target);

        result.Should().BeOfType<MoveResult.Success>();
    }

    [Fact]
    public void GivenItemAtSource_AndEmptyTarget_WhenMoveItem_ThenSourceBecomesEmptyAndTargetHasItem()
    {
        MergeGrid sut = new MergeGrid(5, 5);
        GridPosition source = new GridPosition(0, 0);
        GridPosition target = new GridPosition(3, 3);
        sut.PlaceItem(new MergeItem(new ItemLevel(2), source));

        sut.MoveItem(source, target);

        sut.GetCell(source).Should().Be(CellContent.Empty.Instance);
        sut.GetCell(target).Should().BeOfType<CellContent.Item>()
            .Which.MergeItem.Level.Value.Should().Be(2);
    }

    [Fact]
    public void GivenEmptySource_WhenMoveItem_ThenReturnsFailure()
    {
        MergeGrid sut = new MergeGrid(5, 5);

        MoveResult result = sut.MoveItem(new GridPosition(0, 0), new GridPosition(1, 1));

        result.Should().BeOfType<MoveResult.Failure>();
    }

    [Fact]
    public void GivenItemAtSource_AndOccupiedTarget_WhenMoveItem_ThenReturnsFailure()
    {
        MergeGrid sut = new MergeGrid(5, 5);
        GridPosition source = new GridPosition(0, 0);
        GridPosition target = new GridPosition(1, 0);
        sut.PlaceItem(new MergeItem(new ItemLevel(1), source));
        sut.PlaceItem(new MergeItem(new ItemLevel(1), target));

        MoveResult result = sut.MoveItem(source, target);

        result.Should().BeOfType<MoveResult.Failure>();
    }
}
