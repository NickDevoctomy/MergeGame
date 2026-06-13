using System.Collections.Generic;

using FluentAssertions;

using MergeGame.Domain;

using Xunit;

namespace MergeGame.UnitTests.Domain;

public sealed class MergeGridTests
{
    private readonly StandardMergeRule _rule = new StandardMergeRule();

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
        ItemDefinition def = MakeDef("wood");
        MergeItem item = new MergeItem(def, new GridPosition(2, 2));

        sut.PlaceItem(item);

        CellContent cell = sut.GetCell(new GridPosition(2, 2));
        cell.Should().BeOfType<CellContent.Item>()
            .Which.MergeItem.Definition.Should().Be(def);
    }

    [Fact]
    public void GivenOccupiedCell_WhenPlaceItem_ThenThrowsInvalidOperationException()
    {
        MergeGrid sut = new MergeGrid(5, 5);
        GridPosition pos = new GridPosition(0, 0);
        sut.PlaceItem(new MergeItem(MakeDef("a"), pos));

        Action act = () => sut.PlaceItem(new MergeItem(MakeDef("b"), pos));

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
        ItemDefinition product = MakeDef("sticks", hasProduct: false);
        ItemDefinition chips = new ItemDefinition("chips", string.Empty, "chips.png", product);
        MergeGrid sut = new MergeGrid(5, 5);
        GridPosition sourcePos = new GridPosition(0, 0);
        GridPosition targetPos = new GridPosition(1, 0);
        sut.PlaceItem(new MergeItem(chips, sourcePos));
        sut.PlaceItem(new MergeItem(chips, targetPos));

        MergeResult result = sut.TryMerge(sourcePos, targetPos, _rule);

        result.Should().BeOfType<MergeResult.Success>()
            .Which.ProducedItem.Definition.Should().Be(product);
        sut.GetCell(sourcePos).Should().Be(CellContent.Empty.Instance);
        sut.GetCell(targetPos).Should().BeOfType<CellContent.Item>()
            .Which.MergeItem.Definition.Should().Be(product);
    }

    [Fact]
    public void GivenMismatchedItems_WhenTryMerge_ThenReturnsFailure()
    {
        MergeGrid sut = new MergeGrid(5, 5);
        GridPosition sourcePos = new GridPosition(0, 0);
        GridPosition targetPos = new GridPosition(1, 0);
        sut.PlaceItem(new MergeItem(MakeDef("wood"), sourcePos));
        sut.PlaceItem(new MergeItem(MakeDef("stone"), targetPos));

        MergeResult result = sut.TryMerge(sourcePos, targetPos, _rule);

        result.Should().BeOfType<MergeResult.Failure>();
    }

    [Fact]
    public void GivenEmptySourceCell_WhenTryMerge_ThenReturnsFailure()
    {
        MergeGrid sut = new MergeGrid(5, 5);
        GridPosition targetPos = new GridPosition(1, 0);
        sut.PlaceItem(new MergeItem(MakeDef("wood"), targetPos));

        MergeResult result = sut.TryMerge(new GridPosition(0, 0), targetPos, _rule);

        result.Should().BeOfType<MergeResult.Failure>();
    }

    [Fact]
    public void GivenPartiallyFilledGrid_WhenFindEmptyCells_ThenReturnsOnlyEmptyCells()
    {
        MergeGrid sut = new MergeGrid(2, 2);
        sut.PlaceItem(new MergeItem(MakeDef("wood"), new GridPosition(0, 0)));

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
            new Dictionary<ItemDefinition, int> { [MakeDef("wood")] = 1 });

        sut.PlaceSpawner(pos, def);

        sut.GetCell(pos).Should().BeOfType<CellContent.Spawner>();
    }

    [Fact]
    public void GivenSpawnerCell_WhenFindEmptyCells_ThenSpawnerCellIsNotIncluded()
    {
        MergeGrid sut = new MergeGrid(2, 2);
        GridPosition spawnerPos = new GridPosition(0, 0);
        SpawnerDefinition def = new SpawnerDefinition(
            new Dictionary<ItemDefinition, int> { [MakeDef("wood")] = 1 });
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
        sut.PlaceItem(new MergeItem(MakeDef("wood"), source));

        MoveResult result = sut.MoveItem(source, target);

        result.Should().BeOfType<MoveResult.Success>();
    }

    [Fact]
    public void GivenItemAtSource_AndEmptyTarget_WhenMoveItem_ThenSourceBecomesEmptyAndTargetHasItem()
    {
        MergeGrid sut = new MergeGrid(5, 5);
        GridPosition source = new GridPosition(0, 0);
        GridPosition target = new GridPosition(3, 3);
        ItemDefinition def = MakeDef("stone");
        sut.PlaceItem(new MergeItem(def, source));

        sut.MoveItem(source, target);

        sut.GetCell(source).Should().Be(CellContent.Empty.Instance);
        sut.GetCell(target).Should().BeOfType<CellContent.Item>()
            .Which.MergeItem.Definition.Should().Be(def);
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
        sut.PlaceItem(new MergeItem(MakeDef("a"), source));
        sut.PlaceItem(new MergeItem(MakeDef("b"), target));

        MoveResult result = sut.MoveItem(source, target);

        result.Should().BeOfType<MoveResult.Failure>();
    }

    private static ItemDefinition MakeDef(string name, bool hasProduct = true)
    {
        ItemDefinition? product = hasProduct
            ? new ItemDefinition(name + "_product", string.Empty, name + "_product.png", null)
            : null;

        return new ItemDefinition(name, string.Empty, name + ".png", product);
    }
}
