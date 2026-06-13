using System.Collections.Generic;

using MergeGame.Infrastructure.Tiles;

using Microsoft.Xna.Framework.Graphics;

namespace MergeGame.Game;

/// <summary>Creates, caches, and provides <see cref="Texture2D"/> tiles generated from <see cref="ITileGenerator"/>.</summary>
internal sealed class GameAssetManager
{
    private readonly Dictionary<int, Texture2D> _itemTiles = new Dictionary<int, Texture2D>();
    private readonly GraphicsDevice _graphicsDevice;
    private readonly ITileGenerator _tileGenerator;

    internal GameAssetManager(GraphicsDevice graphicsDevice, ITileGenerator tileGenerator)
    {
        ArgumentNullException.ThrowIfNull(graphicsDevice);
        ArgumentNullException.ThrowIfNull(tileGenerator);

        _graphicsDevice = graphicsDevice;
        _tileGenerator = tileGenerator;

        EmptyTile = CreateTexture(_tileGenerator.GenerateEmptyTile());
        SpawnerTile = CreateTexture(_tileGenerator.GenerateSpawnerTile());
    }

    internal Texture2D EmptyTile { get; }

    internal Texture2D SpawnerTile { get; }

    internal void PreloadTiles(int maxLevel)
    {
        for (int level = 1; level <= maxLevel; level++)
        {
            GetTile(level);
        }
    }

    internal Texture2D GetTile(int level)
    {
        if (!_itemTiles.TryGetValue(level, out Texture2D? texture))
        {
            texture = CreateTexture(_tileGenerator.GenerateTile(level));
            _itemTiles[level] = texture;
        }

        return texture;
    }

    private Texture2D CreateTexture(TileData data)
    {
        var texture = new Texture2D(_graphicsDevice, data.Width, data.Height);
        texture.SetData(data.RgbaPixels);

        return texture;
    }
}
