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
        // Arrange
        MergeGrid sut = new MergeGrid(3, 3);

        // Act
        CellContent cell = sut.GetCell(new GridPosition(1, 1));

        // Assert
        cell.Should().Be(CellContent.Empty.Instance);
    }

    [Fact]
    public void GivenEmptyCell_WhenPlaceItem_ThenCellContainsItem()
    {
        // Arrange
        MergeGrid sut = new MergeGrid(5, 5);
        ItemDefinition def = MakeDef("wood");
        MergeItem item = new MergeItem(def, new GridPosition(2, 2));

        // Act
        sut.PlaceItem(item);

        // Assert
        CellContent cell = sut.GetCell(new GridPosition(2, 2));
        cell.Should().BeOfType<CellContent.Item>()
            .Which.MergeItem.Definition.Should().Be(def);
    }

    [Fact]
    public void GivenOccupiedCell_WhenPlaceItem_ThenThrowsInvalidOperationException()
    {
        // Arrange
        MergeGrid sut = new MergeGrid(5, 5);
        GridPosition pos = new GridPosition(0, 0);
        sut.PlaceItem(new MergeItem(MakeDef("a"), pos));

        // Act
        Action act = () => sut.PlaceItem(new MergeItem(MakeDef("b"), pos));

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GivenOutOfBoundsPosition_WhenGetCell_ThenThrowsArgumentOutOfRangeException()
    {
        // Arrange
        MergeGrid sut = new MergeGrid(3, 3);

        // Act
        Action act = () => sut.GetCell(new GridPosition(5, 5));

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void GivenTwoMatchingItems_WhenTryMerge_ThenReturnsSuccessAndUpdatesGrid()
    {
        // Arrange
        ItemDefinition product = MakeDef("sticks", hasProduct: false);
        ItemDefinition chips = new ItemDefinition("chips", string.Empty, "chips.png", product);
        MergeGrid sut = new MergeGrid(5, 5);
        GridPosition sourcePos = new GridPosition(0, 0);
        GridPosition targetPos = new GridPosition(1, 0);
        sut.PlaceItem(new MergeItem(chips, sourcePos));
        sut.PlaceItem(new MergeItem(chips, targetPos));

        // Act
        MergeResult result = sut.TryMerge(sourcePos, targetPos, _rule);

        // Assert
        result.Should().BeOfType<MergeResult.Success>()
            .Which.ProducedItem.Definition.Should().Be(product);
        sut.GetCell(sourcePos).Should().Be(CellContent.Empty.Instance);
        sut.GetCell(targetPos).Should().BeOfType<CellContent.Item>()
            .Which.MergeItem.Definition.Should().Be(product);
    }

    [Fact]
    public void GivenMismatchedItems_WhenTryMerge_ThenReturnsFailure()
    {
        // Arrange
        MergeGrid sut = new MergeGrid(5, 5);
        GridPosition sourcePos = new GridPosition(0, 0);
        GridPosition targetPos = new GridPosition(1, 0);
        sut.PlaceItem(new MergeItem(MakeDef("wood"), sourcePos));
        sut.PlaceItem(new MergeItem(MakeDef("stone"), targetPos));

        // Act
        MergeResult result = sut.TryMerge(sourcePos, targetPos, _rule);

        // Assert
        result.Should().BeOfType<MergeResult.Failure>();
    }

    [Fact]
    public void GivenEmptySourceCell_WhenTryMerge_ThenReturnsFailure()
    {
        // Arrange
        MergeGrid sut = new MergeGrid(5, 5);
        GridPosition targetPos = new GridPosition(1, 0);
        sut.PlaceItem(new MergeItem(MakeDef("wood"), targetPos));

        // Act
        MergeResult result = sut.TryMerge(new GridPosition(0, 0), targetPos, _rule);

        // Assert
        result.Should().BeOfType<MergeResult.Failure>();
    }

    [Fact]
    public void GivenPartiallyFilledGrid_WhenFindEmptyCells_ThenReturnsOnlyEmptyCells()
    {
        // Arrange
        MergeGrid sut = new MergeGrid(2, 2);
        sut.PlaceItem(new MergeItem(MakeDef("wood"), new GridPosition(0, 0)));

        // Act
        IReadOnlyList<GridPosition> empty = sut.FindEmptyCells();

        // Assert
        empty.Should().HaveCount(3);
        empty.Should().NotContain(new GridPosition(0, 0));
    }

    [Fact]
    public void GivenEmptyCell_WhenPlaceSpawner_ThenCellContainsSpawner()
    {
        // Arrange
        MergeGrid sut = new MergeGrid(5, 5);
        GridPosition pos = new GridPosition(0, 0);
        SpawnerDefinition def = new SpawnerDefinition(
            new Dictionary<ItemDefinition, int> { [MakeDef("wood")] = 1 },
            "Spawner",
            string.Empty,
            "s.png");

        // Act
        sut.PlaceSpawner(pos, def);

        // Assert
        sut.GetCell(pos).Should().BeOfType<CellContent.Spawner>();
    }

    [Fact]
    public void GivenSpawnerCell_WhenFindEmptyCells_ThenSpawnerCellIsNotIncluded()
    {
        // Arrange
        MergeGrid sut = new MergeGrid(2, 2);
        GridPosition spawnerPos = new GridPosition(0, 0);
        SpawnerDefinition def = new SpawnerDefinition(
            new Dictionary<ItemDefinition, int> { [MakeDef("wood")] = 1 },
            "Spawner",
            string.Empty,
            "s.png");
        sut.PlaceSpawner(spawnerPos, def);

        // Act
        IReadOnlyList<GridPosition> empty = sut.FindEmptyCells();

        // Assert
        empty.Should().HaveCount(3);
        empty.Should().NotContain(spawnerPos);
    }

    [Fact]
    public void GivenItemAtSource_AndEmptyTarget_WhenMoveItem_ThenReturnsSuccess()
    {
        // Arrange
        MergeGrid sut = new MergeGrid(5, 5);
        GridPosition source = new GridPosition(0, 0);
        GridPosition target = new GridPosition(3, 3);
        sut.PlaceItem(new MergeItem(MakeDef("wood"), source));

        // Act
        MoveResult result = sut.MoveItem(source, target);

        // Assert
        result.Should().BeOfType<MoveResult.Success>();
    }

    [Fact]
    public void GivenItemAtSource_AndEmptyTarget_WhenMoveItem_ThenSourceBecomesEmptyAndTargetHasItem()
    {
        // Arrange
        MergeGrid sut = new MergeGrid(5, 5);
        GridPosition source = new GridPosition(0, 0);
        GridPosition target = new GridPosition(3, 3);
        ItemDefinition def = MakeDef("stone");
        sut.PlaceItem(new MergeItem(def, source));

        // Act
        sut.MoveItem(source, target);

        // Assert
        sut.GetCell(source).Should().Be(CellContent.Empty.Instance);
        sut.GetCell(target).Should().BeOfType<CellContent.Item>()
            .Which.MergeItem.Definition.Should().Be(def);
    }

    [Fact]
    public void GivenEmptySource_WhenMoveItem_ThenReturnsFailure()
    {
        // Arrange
        MergeGrid sut = new MergeGrid(5, 5);

        // Act
        MoveResult result = sut.MoveItem(new GridPosition(0, 0), new GridPosition(1, 1));

        // Assert
        result.Should().BeOfType<MoveResult.Failure>();
    }

    [Fact]
    public void GivenItemAtSource_AndOccupiedTarget_WhenMoveItem_ThenReturnsFailure()
    {
        // Arrange
        MergeGrid sut = new MergeGrid(5, 5);
        GridPosition source = new GridPosition(0, 0);
        GridPosition target = new GridPosition(1, 0);
        sut.PlaceItem(new MergeItem(MakeDef("a"), source));
        sut.PlaceItem(new MergeItem(MakeDef("b"), target));

        // Act
        MoveResult result = sut.MoveItem(source, target);

        // Assert
        result.Should().BeOfType<MoveResult.Failure>();
    }

    [Fact]
    public void GivenSpawnerCell_WhenRemoveSpawner_ThenCellBecomesEmpty()
    {
        // Arrange
        MergeGrid sut = new MergeGrid(3, 3);
        GridPosition pos = new GridPosition(1, 1);
        sut.PlaceSpawner(pos, MakeSpawnerDef());

        // Act
        sut.RemoveSpawner(pos);

        // Assert
        sut.GetCell(pos).Should().Be(CellContent.Empty.Instance);
    }

    [Fact]
    public void GivenNonSpawnerCell_WhenRemoveSpawner_ThenThrowsInvalidOperationException()
    {
        // Arrange
        MergeGrid sut = new MergeGrid(3, 3);
        GridPosition pos = new GridPosition(0, 0);

        // Act
        Action act = () => sut.RemoveSpawner(pos);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    private static ItemDefinition MakeDef(string name, bool hasProduct = true)
    {
        ItemDefinition? product = hasProduct
            ? new ItemDefinition(name + "_product", string.Empty, name + "_product.png", null)
            : null;

        return new ItemDefinition(name, string.Empty, name + ".png", product);
    }

    private static SpawnerDefinition MakeSpawnerDef()
    {
        return new SpawnerDefinition(
            new Dictionary<ItemDefinition, int> { [MakeDef("wood")] = 1 },
            "Spawner",
            string.Empty,
            "s.png");
    }
}
