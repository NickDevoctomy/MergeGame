using FluentAssertions;

using MergeGame.Domain;

using Xunit;

namespace MergeGame.UnitTests.Domain;

public sealed class ItemDefinitionTests
{
    [Fact]
    public void GivenBackgroundColor_WhenConstructed_ThenBackgroundColorIsStored()
    {
        // Arrange / Act
        ItemDefinition sut = new ItemDefinition("wood", string.Empty, "wood.png", null, "#FF8800");

        // Assert
        sut.BackgroundColor.Should().Be("#FF8800");
    }

    [Fact]
    public void GivenNoBackgroundColor_WhenConstructed_ThenBackgroundColorIsNull()
    {
        // Arrange / Act
        ItemDefinition sut = new ItemDefinition("wood", string.Empty, "wood.png", null);

        // Assert
        sut.BackgroundColor.Should().BeNull();
    }
}
