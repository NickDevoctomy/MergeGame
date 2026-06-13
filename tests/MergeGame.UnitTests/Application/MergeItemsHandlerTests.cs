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
        // Arrange
        ItemDefinition product = MakeDef("sticks", hasProduct: false);
        ItemDefinition chips = new ItemDefinition("chips", string.Empty, "chips.png", product);
        MergeGrid grid = new MergeGrid(5, 5);
        GridPosition sourcePos = new GridPosition(0, 0);
        GridPosition targetPos = new GridPosition(1, 0);
        grid.PlaceItem(new MergeItem(chips, sourcePos));
        grid.PlaceItem(new MergeItem(chips, targetPos));

        MergeItemsHandler sut = new MergeItemsHandler(CreateSession(grid), Substitute.For<ISoundService>());

        // Act
        MergeItemsResult result = sut.Handle(new MergeItemsCommand(sourcePos, targetPos));

        // Assert
        result.Should().BeOfType<MergeItemsResult.Success>()
            .Which.ProducedItem.Definition.Should().Be(product);
    }

    [Fact]
    public void GivenMismatchedItems_WhenHandle_ThenReturnsFailed()
    {
        // Arrange
        MergeGrid grid = new MergeGrid(5, 5);
        GridPosition sourcePos = new GridPosition(0, 0);
        GridPosition targetPos = new GridPosition(1, 0);
        grid.PlaceItem(new MergeItem(MakeDef("wood"), sourcePos));
        grid.PlaceItem(new MergeItem(MakeDef("stone"), targetPos));

        MergeItemsHandler sut = new MergeItemsHandler(CreateSession(grid), Substitute.For<ISoundService>());

        // Act
        MergeItemsResult result = sut.Handle(new MergeItemsCommand(sourcePos, targetPos));

        // Assert
        result.Should().BeOfType<MergeItemsResult.Failed>();
    }

    [Fact]
    public void GivenEmptySourceCell_WhenHandle_ThenReturnsFailed()
    {
        // Arrange
        MergeGrid grid = new MergeGrid(5, 5);
        GridPosition targetPos = new GridPosition(1, 0);
        grid.PlaceItem(new MergeItem(MakeDef("wood"), targetPos));

        MergeItemsHandler sut = new MergeItemsHandler(CreateSession(grid), Substitute.For<ISoundService>());

        // Act
        MergeItemsResult result = sut.Handle(new MergeItemsCommand(new GridPosition(0, 0), targetPos));

        // Assert
        result.Should().BeOfType<MergeItemsResult.Failed>();
    }

    [Fact]
    public void GivenItemsAtFinalTier_WhenHandle_ThenReturnsFailed()
    {
        // Arrange
        ItemDefinition finalDef = MakeDef("plank", hasProduct: false);
        MergeGrid grid = new MergeGrid(5, 5);
        GridPosition sourcePos = new GridPosition(0, 0);
        GridPosition targetPos = new GridPosition(1, 0);
        grid.PlaceItem(new MergeItem(finalDef, sourcePos));
        grid.PlaceItem(new MergeItem(finalDef, targetPos));

        MergeItemsHandler sut = new MergeItemsHandler(CreateSession(grid), Substitute.For<ISoundService>());

        // Act
        MergeItemsResult result = sut.Handle(new MergeItemsCommand(sourcePos, targetPos));

        // Assert
        result.Should().BeOfType<MergeItemsResult.Failed>();
    }

    private static IGameSession CreateSession(MergeGrid grid)
    {
        IGameSession session = Substitute.For<IGameSession>();
        session.Grid.Returns(grid);
        session.MergeRule.Returns(new StandardMergeRule());

        return session;
    }

    private static ItemDefinition MakeDef(string name, bool hasProduct = true)
    {
        ItemDefinition? product = hasProduct
            ? new ItemDefinition(name + "_product", string.Empty, name + "_product.png", null)
            : null;

        return new ItemDefinition(name, string.Empty, name + ".png", product);
    }
}
