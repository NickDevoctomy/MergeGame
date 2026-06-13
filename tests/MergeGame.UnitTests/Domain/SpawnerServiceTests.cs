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
        ItemDefinition def = MakeDef("wood");
        SpawnerDefinition spawnerDef = new SpawnerDefinition(
            new Dictionary<ItemDefinition, int> { [def] = 100 });
        IRandomProvider random = Substitute.For<IRandomProvider>();
        random.GetNext(0, 100).Returns(50);

        ItemDefinition result = SpawnerService.SpawnItem(spawnerDef, random);

        result.Should().Be(def);
    }

    [Fact]
    public void GivenTwoWeights_WhenRollIsInFirstBand_ThenReturnsFirstDefinition()
    {
        ItemDefinition defOne = MakeDef("chips");
        ItemDefinition defTwo = MakeDef("sticks");
        SpawnerDefinition spawnerDef = new SpawnerDefinition(
            new Dictionary<ItemDefinition, int>
            {
                [defOne] = 60,
                [defTwo] = 40,
            });
        IRandomProvider random = Substitute.For<IRandomProvider>();
        random.GetNext(0, 100).Returns(0);

        ItemDefinition result = SpawnerService.SpawnItem(spawnerDef, random);

        result.Should().Be(defOne);
    }

    [Fact]
    public void GivenTwoWeights_WhenRollIsInSecondBand_ThenReturnsSecondDefinition()
    {
        ItemDefinition defOne = MakeDef("chips");
        ItemDefinition defTwo = MakeDef("sticks");
        SpawnerDefinition spawnerDef = new SpawnerDefinition(
            new Dictionary<ItemDefinition, int>
            {
                [defOne] = 60,
                [defTwo] = 40,
            });
        IRandomProvider random = Substitute.For<IRandomProvider>();
        random.GetNext(0, 100).Returns(60);

        ItemDefinition result = SpawnerService.SpawnItem(spawnerDef, random);

        result.Should().Be(defTwo);
    }

    [Fact]
    public void GivenZeroWeightEntry_WhenRollHitsZeroWeightBand_ThenZeroWeightEntryIsSkipped()
    {
        ItemDefinition defOne = MakeDef("chips");
        ItemDefinition defTwo = MakeDef("sticks");
        ItemDefinition defThree = MakeDef("plank");
        SpawnerDefinition spawnerDef = new SpawnerDefinition(
            new Dictionary<ItemDefinition, int>
            {
                [defOne] = 0,
                [defTwo] = 50,
                [defThree] = 50,
            });
        IRandomProvider random = Substitute.For<IRandomProvider>();
        random.GetNext(0, 100).Returns(0);

        ItemDefinition result = SpawnerService.SpawnItem(spawnerDef, random);

        result.Should().NotBe(defOne);
    }

    private static ItemDefinition MakeDef(string name)
    {
        return new ItemDefinition(name, string.Empty, name + ".png", null);
    }
}
