using System.Collections.Generic;

using FluentAssertions;

using MergeGame.Infrastructure.Config;
using MergeGame.Infrastructure.Tiles;

using Xunit;

namespace MergeGame.UnitTests.Infrastructure;

public sealed class BmpTileGeneratorTests
{
    [Fact]
    public void GivenLevel1_WhenGenerateTile_ThenTileHasCorrectDimensions()
    {
        BmpTileGenerator sut = new BmpTileGenerator(CreateConfig(64));

        TileData result = sut.GenerateTile(1);

        result.Width.Should().Be(64);
        result.Height.Should().Be(64);
        result.RgbaPixels.Should().HaveCount(64 * 64 * 4);
    }

    [Fact]
    public void GivenLevel1WithKnownColor_WhenGenerateTile_ThenCenterPixelMatchesExpectedColor()
    {
        BmpTileGenerator sut = new BmpTileGenerator(CreateConfig(32));

        TileData result = sut.GenerateTile(1);

        (byte er, byte eg, byte eb) = BmpTileGenerator.ParseHexColor("#FF4444");
        int cx = 2; // First interior column (BorderWidth = 2)
        int cy = 2; // First interior row
        int idx = ((cy * 32) + cx) * 4;

        result.RgbaPixels[idx + 0].Should().Be(er);
        result.RgbaPixels[idx + 1].Should().Be(eg);
        result.RgbaPixels[idx + 2].Should().Be(eb);
        result.RgbaPixels[idx + 3].Should().Be(255);
    }

    [Fact]
    public void GivenUnknownLevel_WhenGenerateTile_ThenTileIsGeneratedWithoutThrowing()
    {
        BmpTileGenerator sut = new BmpTileGenerator(CreateConfig());

        Action act = () => sut.GenerateTile(99);

        act.Should().NotThrow();
    }

    [Fact]
    public void GivenZeroLevel_WhenGenerateTile_ThenThrowsArgumentOutOfRangeException()
    {
        BmpTileGenerator sut = new BmpTileGenerator(CreateConfig());

        Action act = () => sut.GenerateTile(0);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void WhenGenerateEmptyTile_ThenTileHasCorrectDimensions()
    {
        BmpTileGenerator sut = new BmpTileGenerator(CreateConfig(64));

        TileData result = sut.GenerateEmptyTile();

        result.Width.Should().Be(64);
        result.Height.Should().Be(64);
    }

    [Fact]
    public void WhenGenerateSpawnerTile_ThenTileHasCorrectDimensions()
    {
        BmpTileGenerator sut = new BmpTileGenerator(CreateConfig(64));

        TileData result = sut.GenerateSpawnerTile();

        result.Width.Should().Be(64);
        result.Height.Should().Be(64);
    }

    [Fact]
    public void GivenValidHexColor_WhenParseHexColor_ThenReturnsCorrectChannels()
    {
        (byte r, byte g, byte b) = BmpTileGenerator.ParseHexColor("#FF8844");

        r.Should().Be(0xFF);
        g.Should().Be(0x88);
        b.Should().Be(0x44);
    }

    [Fact]
    public void GivenAllLevelsDifferent_WhenGenerateTiles_ThenEachTileTopRowColorDiffersFromAdjacent()
    {
        TileConfig config = new TileConfig { TileSize = 16, MaxLevel = 5, LevelColors = new Dictionary<int, string>() };
        BmpTileGenerator sut = new BmpTileGenerator(config);

        TileData tile1 = sut.GenerateTile(1);
        TileData tile2 = sut.GenerateTile(2);

        // R channel is the same for adjacent HSL hues; compare G which differs
        int interiorIdx = ((2 * 16) + 2) * 4; // row=2, col=2 for tileSize=16
        tile1.RgbaPixels[interiorIdx + 1].Should().NotBe(tile2.RgbaPixels[interiorIdx + 1]);
    }

    private static TileConfig CreateConfig(int tileSize = 32)
    {
        return new TileConfig
        {
            TileSize = tileSize,
            MaxLevel = 10,
            LevelColors = new Dictionary<int, string>
            {
                [1] = "#FF4444",
                [2] = "#FF8844",
            },
        };
    }
}
