using System;
using System.Collections.Generic;

using FluentAssertions;

using MergeGame.Domain;

using Xunit;

namespace MergeGame.UnitTests.Domain;

public sealed class SpawnerDefinitionTests
{
    [Fact]
    public void GivenValidWeights_WhenConstructed_ThenWeightsAreStored()
    {
        // Arrange
        ItemDefinition def = new ItemDefinition("wood", string.Empty, "wood.png", null);
        var weights = new Dictionary<ItemDefinition, int> { [def] = 10 };

        // Act
        SpawnerDefinition sut = new SpawnerDefinition(weights, "Spawner", string.Empty, "s.png");

        // Assert
        sut.Weights.Should().ContainKey(def).WhoseValue.Should().Be(10);
    }

    [Fact]
    public void GivenNullWeights_WhenConstructed_ThenThrowsArgumentNullException()
    {
        // Arrange / Act
        Action act = () => _ = new SpawnerDefinition(null!, "Spawner", string.Empty, "s.png");

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenEmptyWeights_WhenConstructed_ThenThrowsArgumentException()
    {
        // Arrange
        var weights = new Dictionary<ItemDefinition, int>();

        // Act
        Action act = () => _ = new SpawnerDefinition(weights, "Spawner", string.Empty, "s.png");

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*at least one*");
    }

    [Fact]
    public void GivenAllZeroWeights_WhenConstructed_ThenThrowsArgumentException()
    {
        // Arrange
        ItemDefinition def = new ItemDefinition("stone", string.Empty, "stone.png", null);
        var weights = new Dictionary<ItemDefinition, int> { [def] = 0 };

        // Act
        Action act = () => _ = new SpawnerDefinition(weights, "Spawner", string.Empty, "s.png");

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*greater than zero*");
    }

    [Fact]
    public void GivenEmptyName_WhenConstructed_ThenThrowsArgumentException()
    {
        // Arrange
        ItemDefinition def = new ItemDefinition("wood", string.Empty, "wood.png", null);
        var weights = new Dictionary<ItemDefinition, int> { [def] = 1 };

        // Act
        Action act = () => _ = new SpawnerDefinition(weights, string.Empty, string.Empty, "s.png");

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenNegativeItemLimit_WhenConstructed_ThenThrowsArgumentOutOfRangeException()
    {
        // Arrange
        ItemDefinition def = new ItemDefinition("wood", string.Empty, "wood.png", null);
        var weights = new Dictionary<ItemDefinition, int> { [def] = 1 };

        // Act
        Action act = () => _ = new SpawnerDefinition(weights, "Spawner", string.Empty, "s.png", itemLimit: -1);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void GivenNewSpawner_WhenNotActivated_ThenIsExhaustedIsFalse()
    {
        // Arrange
        SpawnerDefinition sut = MakeDefinition(itemLimit: 3);

        // Act / Assert
        sut.IsExhausted.Should().BeFalse();
    }

    [Fact]
    public void GivenFiniteSpawner_WhenRecordSpawnCalledUpToLimit_ThenIsExhaustedIsTrue()
    {
        // Arrange
        SpawnerDefinition sut = MakeDefinition(itemLimit: 2);

        // Act
        sut.RecordSpawn();
        sut.RecordSpawn();

        // Assert
        sut.IsExhausted.Should().BeTrue();
    }

    [Fact]
    public void GivenFiniteSpawner_WhenRecordSpawnCalledBelowLimit_ThenIsExhaustedIsFalse()
    {
        // Arrange
        SpawnerDefinition sut = MakeDefinition(itemLimit: 3);

        // Act
        sut.RecordSpawn();

        // Assert
        sut.IsExhausted.Should().BeFalse();
    }

    [Fact]
    public void GivenUnlimitedSpawner_WhenRecordSpawnCalledManyTimes_ThenIsExhaustedIsFalse()
    {
        // Arrange
        SpawnerDefinition sut = MakeDefinition(itemLimit: 0);

        // Act
        for (int i = 0; i < 1000; i++)
        {
            sut.RecordSpawn();
        }

        // Assert
        sut.IsExhausted.Should().BeFalse();
    }

    [Fact]
    public void GivenRecordSpawn_WhenCalled_ThenSpawnCountIncrements()
    {
        // Arrange
        SpawnerDefinition sut = MakeDefinition();

        // Act
        sut.RecordSpawn();
        sut.RecordSpawn();

        // Assert
        sut.SpawnCount.Should().Be(2);
    }

    private static SpawnerDefinition MakeDefinition(int itemLimit = 0)
    {
        ItemDefinition def = new ItemDefinition("wood", string.Empty, "wood.png", null);
        var weights = new Dictionary<ItemDefinition, int> { [def] = 1 };

        return new SpawnerDefinition(weights, "Test Spawner", string.Empty, "s.png", itemLimit);
    }
}
