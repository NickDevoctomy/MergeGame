namespace MergeGame.Infrastructure.Tiles;

/// <summary>Generates raw RGBA tile images for use by the renderer.</summary>
public interface ITileGenerator
{
    /// <summary>Generates a tile for an item at the given level (1 or higher).</summary>
    /// <param name="level">The item level; must be 1 or higher.</param>
    /// <returns>A <see cref="TileData"/> containing the RGBA pixel data for the tile.</returns>
    public TileData GenerateTile(int level);

    /// <summary>Generates the tile used for empty grid cells.</summary>
    /// <returns>A <see cref="TileData"/> for an empty cell.</returns>
    public TileData GenerateEmptyTile();

    /// <summary>Generates the tile used for spawner cells.</summary>
    /// <returns>A <see cref="TileData"/> for a spawner cell.</returns>
    public TileData GenerateSpawnerTile();
}
