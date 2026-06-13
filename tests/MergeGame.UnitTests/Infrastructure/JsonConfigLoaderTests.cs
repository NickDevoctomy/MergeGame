using FluentAssertions;

using MergeGame.Infrastructure.Config;

using Xunit;

namespace MergeGame.UnitTests.Infrastructure;

public sealed class JsonConfigLoaderTests
{
    private const string ValidJson = """
        {
          "grid": { "columns": 10, "rows": 10 },
          "tileSize": 64,
          "items": [
            {
              "name": "Wood Chips",
              "description": "Scraps",
              "image": "res/chips.png",
              "product": {
                "name": "Wood Sticks",
                "description": "Sticks",
                "image": "res/sticks.png",
                "product": null
              }
            }
          ],
          "spawners": [
            {
              "column": 0,
              "row": 0,
              "spawnableItems": [
                { "itemName": "Wood Chips", "weight": 100 }
              ]
            }
          ],
          "sounds": { "enabled": false, "soundFiles": {} }
        }
        """;

    [Fact]
    public void GivenValidJson_WhenParseConfig_ThenReturnsPopulatedConfig()
    {
        GameConfig result = JsonConfigLoader.ParseConfig(ValidJson);

        result.Grid.Columns.Should().Be(10);
        result.Grid.Rows.Should().Be(10);
        result.TileSize.Should().Be(64);
        result.Items.Should().HaveCount(1);
        result.Items[0].Name.Should().Be("Wood Chips");
        result.Items[0].Product!.Name.Should().Be("Wood Sticks");
        result.Spawners.Should().HaveCount(1);
        result.Spawners[0].SpawnableItems[0].ItemName.Should().Be("Wood Chips");
        result.Spawners[0].SpawnableItems[0].Weight.Should().Be(100);
    }

    [Fact]
    public void GivenJsonWithZeroColumns_WhenParseConfig_ThenThrowsConfigurationException()
    {
        const string Json = """
            {
              "grid": { "columns": 0, "rows": 10 },
              "tileSize": 64,
              "items": [{ "name": "a", "description": "", "image": "a.png", "product": null }],
              "spawners": [],
              "sounds": { "enabled": false, "soundFiles": {} }
            }
            """;

        Action act = () => JsonConfigLoader.ParseConfig(Json);

        act.Should().Throw<ConfigurationException>().WithMessage("*columns*");
    }

    [Fact]
    public void GivenJsonWithZeroTileSize_WhenParseConfig_ThenThrowsConfigurationException()
    {
        const string Json = """
            {
              "grid": { "columns": 10, "rows": 10 },
              "tileSize": 0,
              "items": [{ "name": "a", "description": "", "image": "a.png", "product": null }],
              "spawners": [],
              "sounds": { "enabled": false, "soundFiles": {} }
            }
            """;

        Action act = () => JsonConfigLoader.ParseConfig(Json);

        act.Should().Throw<ConfigurationException>().WithMessage("*tileSize*");
    }

    [Fact]
    public void GivenJsonWithDuplicateItemNames_WhenParseConfig_ThenThrowsConfigurationException()
    {
        const string Json = """
            {
              "grid": { "columns": 10, "rows": 10 },
              "tileSize": 64,
              "items": [
                { "name": "chips", "description": "", "image": "a.png", "product": null },
                { "name": "chips", "description": "", "image": "b.png", "product": null }
              ],
              "spawners": [],
              "sounds": { "enabled": false, "soundFiles": {} }
            }
            """;

        Action act = () => JsonConfigLoader.ParseConfig(Json);

        act.Should().Throw<ConfigurationException>().WithMessage("*Duplicate*");
    }

    [Fact]
    public void GivenMalformedJson_WhenParseConfig_ThenThrowsConfigurationException()
    {
        const string Json = "{ this is not valid json }";

        Action act = () => JsonConfigLoader.ParseConfig(Json);

        act.Should().Throw<ConfigurationException>().WithMessage("*malformed*");
    }

    [Fact]
    public void GivenSpawnerWithAllZeroWeights_WhenParseConfig_ThenThrowsConfigurationException()
    {
        const string Json = """
            {
              "grid": { "columns": 10, "rows": 10 },
              "tileSize": 64,
              "items": [{ "name": "chips", "description": "", "image": "a.png", "product": null }],
              "spawners": [
                { "column": 0, "row": 0, "spawnableItems": [{ "itemName": "chips", "weight": 0 }] }
              ],
              "sounds": { "enabled": false, "soundFiles": {} }
            }
            """;

        Action act = () => JsonConfigLoader.ParseConfig(Json);

        act.Should().Throw<ConfigurationException>().WithMessage("*weight*");
    }

    [Fact]
    public void GivenSpawnerReferencingUnknownItem_WhenParseConfig_ThenThrowsConfigurationException()
    {
        const string Json = """
            {
              "grid": { "columns": 10, "rows": 10 },
              "tileSize": 64,
              "items": [{ "name": "chips", "description": "", "image": "a.png", "product": null }],
              "spawners": [
                { "column": 0, "row": 0, "spawnableItems": [{ "itemName": "unknown", "weight": 10 }] }
              ],
              "sounds": { "enabled": false, "soundFiles": {} }
            }
            """;

        Action act = () => JsonConfigLoader.ParseConfig(Json);

        act.Should().Throw<ConfigurationException>().WithMessage("*unknown*");
    }
}
