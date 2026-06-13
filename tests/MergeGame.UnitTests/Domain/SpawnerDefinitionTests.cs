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
        ItemDefinition def = new ItemDefinition("wood", string.Empty, "wood.png", null);
        var weights = new Dictionary<ItemDefinition, int> { [def] = 10 };

        SpawnerDefinition sut = new SpawnerDefinition(weights, "Spawner", string.Empty, "s.png");

        sut.Weights.Should().ContainKey(def).WhoseValue.Should().Be(10);
    }

    [Fact]
    public void GivenNullWeights_WhenConstructed_ThenThrowsArgumentNullException()
    {
        Action act = () => _ = new SpawnerDefinition(null!, "Spawner", string.Empty, "s.png");

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenEmptyWeights_WhenConstructed_ThenThrowsArgumentException()
    {
        var weights = new Dictionary<ItemDefinition, int>();

        Action act = () => _ = new SpawnerDefinition(weights, "Spawner", string.Empty, "s.png");

        act.Should().Throw<ArgumentException>().WithMessage("*at least one*");
    }

    [Fact]
    public void GivenAllZeroWeights_WhenConstructed_ThenThrowsArgumentException()
    {
        ItemDefinition def = new ItemDefinition("stone", string.Empty, "stone.png", null);
        var weights = new Dictionary<ItemDefinition, int> { [def] = 0 };

        Action act = () => _ = new SpawnerDefinition(weights, "Spawner", string.Empty, "s.png");

        act.Should().Throw<ArgumentException>().WithMessage("*greater than zero*");
    }

    [Fact]
    public void GivenEmptyName_WhenConstructed_ThenThrowsArgumentException()
    {
        ItemDefinition def = new ItemDefinition("wood", string.Empty, "wood.png", null);
        var weights = new Dictionary<ItemDefinition, int> { [def] = 1 };

        Action act = () => _ = new SpawnerDefinition(weights, string.Empty, string.Empty, "s.png");

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenNegativeItemLimit_WhenConstructed_ThenThrowsArgumentOutOfRangeException()
    {
        ItemDefinition def = new ItemDefinition("wood", string.Empty, "wood.png", null);
        var weights = new Dictionary<ItemDefinition, int> { [def] = 1 };

        Action act = () => _ = new SpawnerDefinition(weights, "Spawner", string.Empty, "s.png", itemLimit: -1);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void GivenNewSpawner_WhenNotActivated_ThenIsExhaustedIsFalse()
    {
        SpawnerDefinition sut = MakeDefinition(itemLimit: 3);

        sut.IsExhausted.Should().BeFalse();
    }

    [Fact]
    public void GivenFiniteSpawner_WhenRecordSpawnCalledUpToLimit_ThenIsExhaustedIsTrue()
    {
        SpawnerDefinition sut = MakeDefinition(itemLimit: 2);

        sut.RecordSpawn();
        sut.RecordSpawn();

        sut.IsExhausted.Should().BeTrue();
    }

    [Fact]
    public void GivenFiniteSpawner_WhenRecordSpawnCalledBelowLimit_ThenIsExhaustedIsFalse()
    {
        SpawnerDefinition sut = MakeDefinition(itemLimit: 3);

        sut.RecordSpawn();

        sut.IsExhausted.Should().BeFalse();
    }

    [Fact]
    public void GivenUnlimitedSpawner_WhenRecordSpawnCalledManyTimes_ThenIsExhaustedIsFalse()
    {
        SpawnerDefinition sut = MakeDefinition(itemLimit: 0);

        for (int i = 0; i < 1000; i++)
        {
            sut.RecordSpawn();
        }

        sut.IsExhausted.Should().BeFalse();
    }

    [Fact]
    public void GivenRecordSpawn_WhenCalled_ThenSpawnCountIncrements()
    {
        SpawnerDefinition sut = MakeDefinition();

        sut.RecordSpawn();
        sut.RecordSpawn();

        sut.SpawnCount.Should().Be(2);
    }

    private static SpawnerDefinition MakeDefinition(int itemLimit = 0)
    {
        ItemDefinition def = new ItemDefinition("wood", string.Empty, "wood.png", null);
        var weights = new Dictionary<ItemDefinition, int> { [def] = 1 };

        return new SpawnerDefinition(weights, "Test Spawner", string.Empty, "s.png", itemLimit);
    }
}
