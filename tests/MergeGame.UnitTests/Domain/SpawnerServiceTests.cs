using System.Collections.Generic;

using FluentAssertions;

using MergeGame.Domain;

using NSubstitute;

using Xunit;

namespace MergeGame.UnitTests.Domain;

public sealed class SpawnerServiceTests
{
    private readonly SpawnerService _sut = new SpawnerService();

    [Fact]
    public void GivenSingleWeightEntry_WhenSpawnItem_ThenAlwaysReturnsOnlyLevel()
    {
        ItemLevel level = new ItemLevel(1);
        SpawnerDefinition def = new SpawnerDefinition(
            new Dictionary<ItemLevel, int> { [level] = 100 });
        IRandomProvider random = Substitute.For<IRandomProvider>();
        random.GetNext(0, 100).Returns(50);

        ItemLevel result = SpawnerService.SpawnItem(def, random);

        result.Should().Be(level);
    }

    [Fact]
    public void GivenTwoWeights_WhenRollIsInFirstBand_ThenReturnsFirstLevel()
    {
        ItemLevel levelOne = new ItemLevel(1);
        ItemLevel levelTwo = new ItemLevel(2);
        SpawnerDefinition def = new SpawnerDefinition(
            new Dictionary<ItemLevel, int>
            {
                [levelOne] = 60,
                [levelTwo] = 40,
            });
        IRandomProvider random = Substitute.For<IRandomProvider>();
        random.GetNext(0, 100).Returns(0);

        ItemLevel result = SpawnerService.SpawnItem(def, random);

        result.Should().Be(levelOne);
    }

    [Fact]
    public void GivenTwoWeights_WhenRollIsInSecondBand_ThenReturnsSecondLevel()
    {
        ItemLevel levelOne = new ItemLevel(1);
        ItemLevel levelTwo = new ItemLevel(2);
        SpawnerDefinition def = new SpawnerDefinition(
            new Dictionary<ItemLevel, int>
            {
                [levelOne] = 60,
                [levelTwo] = 40,
            });
        IRandomProvider random = Substitute.For<IRandomProvider>();
        random.GetNext(0, 100).Returns(60);

        ItemLevel result = SpawnerService.SpawnItem(def, random);

        result.Should().Be(levelTwo);
    }

    [Fact]
    public void GivenZeroWeightEntry_WhenRollHitsZeroWeightBand_ThenZeroWeightEntryIsSkipped()
    {
        ItemLevel levelOne = new ItemLevel(1);
        ItemLevel levelTwo = new ItemLevel(2);
        ItemLevel levelThree = new ItemLevel(3);
        SpawnerDefinition def = new SpawnerDefinition(
            new Dictionary<ItemLevel, int>
            {
                [levelOne] = 0,
                [levelTwo] = 50,
                [levelThree] = 50,
            });
        IRandomProvider random = Substitute.For<IRandomProvider>();
        random.GetNext(0, 100).Returns(0);

        ItemLevel result = SpawnerService.SpawnItem(def, random);

        result.Should().NotBe(levelOne);
    }
}
