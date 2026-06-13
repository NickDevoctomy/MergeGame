using FluentAssertions;

using MergeGame.Domain;

using Xunit;

namespace MergeGame.UnitTests.Domain;

public sealed class StandardMergeRuleTests
{
    private readonly StandardMergeRule _sut = new StandardMergeRule();

    [Fact]
    public void GivenTwoItemsWithSameDefinition_WhenCanMerge_ThenReturnsTrue()
    {
        // Arrange
        ItemDefinition def = MakeDef("wood", hasProduct: true);
        MergeItem a = new MergeItem(def, new GridPosition(0, 0));
        MergeItem b = new MergeItem(def, new GridPosition(1, 0));

        // Act
        bool result = _sut.CanMerge(a, b);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void GivenTwoItemsWithDifferentDefinitions_WhenCanMerge_ThenReturnsFalse()
    {
        // Arrange
        MergeItem a = new MergeItem(MakeDef("wood", hasProduct: true), new GridPosition(0, 0));
        MergeItem b = new MergeItem(MakeDef("stone", hasProduct: true), new GridPosition(1, 0));

        // Act
        bool result = _sut.CanMerge(a, b);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GivenBothItemsAtFinalTier_WhenCanMerge_ThenReturnsFalse()
    {
        // Arrange
        ItemDefinition finalDef = MakeDef("plank", hasProduct: false);
        MergeItem a = new MergeItem(finalDef, new GridPosition(0, 0));
        MergeItem b = new MergeItem(finalDef, new GridPosition(1, 0));

        // Act
        bool result = _sut.CanMerge(a, b);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GivenTwoMergeableItems_WhenMerge_ThenProducesItemWithProductDefinition()
    {
        // Arrange
        ItemDefinition product = MakeDef("sticks", hasProduct: false);
        ItemDefinition source = new ItemDefinition("chips", string.Empty, "chips.png", product);
        MergeItem a = new MergeItem(source, new GridPosition(0, 0));
        MergeItem b = new MergeItem(source, new GridPosition(1, 0));
        GridPosition target = new GridPosition(1, 0);

        // Act
        MergeItem result = _sut.Merge(a, b, target);

        // Assert
        result.Definition.Should().Be(product);
        result.Position.Should().Be(target);
    }

    [Fact]
    public void GivenNonMergeableItems_WhenMerge_ThenThrowsInvalidOperationException()
    {
        // Arrange
        MergeItem a = new MergeItem(MakeDef("wood", hasProduct: true), new GridPosition(0, 0));
        MergeItem b = new MergeItem(MakeDef("stone", hasProduct: true), new GridPosition(1, 0));

        // Act
        Action act = () => _sut.Merge(a, b, new GridPosition(1, 0));

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    private static ItemDefinition MakeDef(string name, bool hasProduct)
    {
        ItemDefinition? product = hasProduct
            ? new ItemDefinition(name + "_product", string.Empty, name + "_product.png", null)
            : null;

        return new ItemDefinition(name, string.Empty, name + ".png", product);
    }
}
