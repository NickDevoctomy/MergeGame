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

        SpawnerDefinition sut = new SpawnerDefinition(weights);

        sut.Weights.Should().ContainKey(def).WhoseValue.Should().Be(10);
    }

    [Fact]
    public void GivenNullWeights_WhenConstructed_ThenThrowsArgumentNullException()
    {
        Action act = () => _ = new SpawnerDefinition(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenEmptyWeights_WhenConstructed_ThenThrowsArgumentException()
    {
        var weights = new Dictionary<ItemDefinition, int>();

        Action act = () => _ = new SpawnerDefinition(weights);

        act.Should().Throw<ArgumentException>().WithMessage("*at least one*");
    }

    [Fact]
    public void GivenAllZeroWeights_WhenConstructed_ThenThrowsArgumentException()
    {
        ItemDefinition def = new ItemDefinition("stone", string.Empty, "stone.png", null);
        var weights = new Dictionary<ItemDefinition, int> { [def] = 0 };

        Action act = () => _ = new SpawnerDefinition(weights);

        act.Should().Throw<ArgumentException>().WithMessage("*greater than zero*");
    }
}
