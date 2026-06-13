using System.Collections.Generic;

using FluentAssertions;

using MergeGame.Domain;

using NSubstitute;

using Xunit;

namespace MergeGame.UnitTests.Domain;

public sealed class SpawnerServiceTests
{
    [Fact]
    public void GivenSingleWeightEntry_WhenSpawnItem_ThenAlwaysReturnsOnlyDefinition()
    {
        // Arrange
        ItemDefinition def = MakeDef("wood");
        SpawnerDefinition spawnerDef = new SpawnerDefinition(
            new Dictionary<ItemDefinition, int> { [def] = 100 },
            "Test",
            string.Empty,
            "s.png");
        IRandomProvider random = Substitute.For<IRandomProvider>();
        random.GetNext(0, 100).Returns(50);

        // Act
        ItemDefinition result = SpawnerService.SpawnItem(spawnerDef, random);

        // Assert
        result.Should().Be(def);
    }

    [Fact]
    public void GivenTwoWeights_WhenRollIsInFirstBand_ThenReturnsFirstDefinition()
    {
        // Arrange
        ItemDefinition defOne = MakeDef("chips");
        ItemDefinition defTwo = MakeDef("sticks");
        SpawnerDefinition spawnerDef = new SpawnerDefinition(
            new Dictionary<ItemDefinition, int>
            {
                [defOne] = 60,
                [defTwo] = 40,
            },
            "Test",
            string.Empty,
            "s.png");
        IRandomProvider random = Substitute.For<IRandomProvider>();
        random.GetNext(0, 100).Returns(0);

        // Act
        ItemDefinition result = SpawnerService.SpawnItem(spawnerDef, random);

        // Assert
        result.Should().Be(defOne);
    }

    [Fact]
    public void GivenTwoWeights_WhenRollIsInSecondBand_ThenReturnsSecondDefinition()
    {
        // Arrange
        ItemDefinition defOne = MakeDef("chips");
        ItemDefinition defTwo = MakeDef("sticks");
        SpawnerDefinition spawnerDef = new SpawnerDefinition(
            new Dictionary<ItemDefinition, int>
            {
                [defOne] = 60,
                [defTwo] = 40,
            },
            "Test",
            string.Empty,
            "s.png");
        IRandomProvider random = Substitute.For<IRandomProvider>();
        random.GetNext(0, 100).Returns(60);

        // Act
        ItemDefinition result = SpawnerService.SpawnItem(spawnerDef, random);

        // Assert
        result.Should().Be(defTwo);
    }

    [Fact]
    public void GivenZeroWeightEntry_WhenRollHitsZeroWeightBand_ThenZeroWeightEntryIsSkipped()
    {
        // Arrange
        ItemDefinition defOne = MakeDef("chips");
        ItemDefinition defTwo = MakeDef("sticks");
        ItemDefinition defThree = MakeDef("plank");
        SpawnerDefinition spawnerDef = new SpawnerDefinition(
            new Dictionary<ItemDefinition, int>
            {
                [defOne] = 0,
                [defTwo] = 50,
                [defThree] = 50,
            },
            "Test",
            string.Empty,
            "s.png");
        IRandomProvider random = Substitute.For<IRandomProvider>();
        random.GetNext(0, 100).Returns(0);

        // Act
        ItemDefinition result = SpawnerService.SpawnItem(spawnerDef, random);

        // Assert
        result.Should().NotBe(defOne);
    }

    private static ItemDefinition MakeDef(string name)
    {
        return new ItemDefinition(name, string.Empty, name + ".png", null);
    }
}
