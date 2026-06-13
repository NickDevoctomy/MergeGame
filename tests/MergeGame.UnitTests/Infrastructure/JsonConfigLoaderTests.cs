using FluentAssertions;

using MergeGame.Infrastructure.Config;

using Xunit;

namespace MergeGame.UnitTests.Infrastructure;

public sealed class JsonConfigLoaderTests
{
    [Fact]
    public void GivenValidJson_WhenParseConfig_ThenReturnsPopulatedConfig()
    {
        const string Json = """
            {
              "grid": { "columns": 10, "rows": 10 },
              "spawners": [{ "column": 0, "row": 0, "weights": { "1": 100 } }],
              "tiles": { "tileSize": 64, "maxLevel": 10, "levelColors": { "1": "#FF0000" } },
              "sounds": { "enabled": false, "soundFiles": {} }
            }
            """;

        GameConfig result = JsonConfigLoader.ParseConfig(Json);

        result.Grid.Columns.Should().Be(10);
        result.Grid.Rows.Should().Be(10);
        result.Tiles.TileSize.Should().Be(64);
        result.Tiles.MaxLevel.Should().Be(10);
        result.Spawners.Should().HaveCount(1);
        result.Spawners[0].Column.Should().Be(0);
        result.Spawners[0].Weights[1].Should().Be(100);
    }

    [Fact]
    public void GivenJsonWithZeroColumns_WhenParseConfig_ThenThrowsConfigurationException()
    {
        const string Json = """
            {
              "grid": { "columns": 0, "rows": 10 },
              "spawners": [],
              "tiles": { "tileSize": 64, "maxLevel": 10, "levelColors": {} },
              "sounds": { "enabled": false, "soundFiles": {} }
            }
            """;

        Action act = () => JsonConfigLoader.ParseConfig(Json);

        act.Should().Throw<ConfigurationException>()
            .WithMessage("*columns*");
    }

    [Fact]
    public void GivenJsonWithZeroTileSize_WhenParseConfig_ThenThrowsConfigurationException()
    {
        const string Json = """
            {
              "grid": { "columns": 10, "rows": 10 },
              "spawners": [],
              "tiles": { "tileSize": 0, "maxLevel": 10, "levelColors": {} },
              "sounds": { "enabled": false, "soundFiles": {} }
            }
            """;

        Action act = () => JsonConfigLoader.ParseConfig(Json);

        act.Should().Throw<ConfigurationException>()
            .WithMessage("*tileSize*");
    }

    [Fact]
    public void GivenInvalidHexColor_WhenParseConfig_ThenThrowsConfigurationException()
    {
        const string Json = """
            {
              "grid": { "columns": 10, "rows": 10 },
              "spawners": [],
              "tiles": { "tileSize": 64, "maxLevel": 10, "levelColors": { "1": "NOTACOLOR" } },
              "sounds": { "enabled": false, "soundFiles": {} }
            }
            """;

        Action act = () => JsonConfigLoader.ParseConfig(Json);

        act.Should().Throw<ConfigurationException>();
    }

    [Fact]
    public void GivenMalformedJson_WhenParseConfig_ThenThrowsConfigurationException()
    {
        const string Json = "{ this is not valid json }";

        Action act = () => JsonConfigLoader.ParseConfig(Json);

        act.Should().Throw<ConfigurationException>()
            .WithMessage("*malformed*");
    }

    [Fact]
    public void GivenSpawnerWithAllZeroWeights_WhenParseConfig_ThenThrowsConfigurationException()
    {
        const string Json = """
            {
              "grid": { "columns": 10, "rows": 10 },
              "spawners": [{ "column": 0, "row": 0, "weights": { "1": 0 } }],
              "tiles": { "tileSize": 64, "maxLevel": 10, "levelColors": {} },
              "sounds": { "enabled": false, "soundFiles": {} }
            }
            """;

        Action act = () => JsonConfigLoader.ParseConfig(Json);

        act.Should().Throw<ConfigurationException>()
            .WithMessage("*weight*");
    }
}
