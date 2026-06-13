using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework.Graphics;

namespace MergeGame.Game;

/// <summary>Loads, caches, and provides <see cref="Texture2D"/> tiles for the merge grid.</summary>
internal sealed class GameAssetManager
{
    private readonly Dictionary<string, Texture2D> _itemTiles = new Dictionary<string, Texture2D>();
    private readonly GraphicsDevice _graphicsDevice;
    private readonly int _tileSize;

    internal GameAssetManager(GraphicsDevice graphicsDevice, int tileSize)
    {
        ArgumentNullException.ThrowIfNull(graphicsDevice);

        _graphicsDevice = graphicsDevice;
        _tileSize = tileSize;

        EmptyTile = CreateColoredTile(0x2A, 0x2A, 0x2A);
        SpawnerTile = CreateColoredTile(0x1A, 0x4A, 0x1A);
    }

    internal Texture2D EmptyTile { get; }

    internal Texture2D SpawnerTile { get; }

    internal Texture2D GetTile(string imagePath)
    {
        if (!_itemTiles.TryGetValue(imagePath, out Texture2D? texture))
        {
            texture = LoadFromFile(imagePath);
            _itemTiles[imagePath] = texture;
        }

        return texture;
    }

    private Texture2D LoadFromFile(string path)
    {
        string fullPath = Path.IsPathRooted(path)
            ? path
            : Path.Combine(AppContext.BaseDirectory, path);

        if (!File.Exists(fullPath))
        {
            return CreateColoredTile(0x80, 0x80, 0x80);
        }

        using FileStream stream = File.OpenRead(fullPath);

        return Texture2D.FromStream(_graphicsDevice, stream);
    }

    private Texture2D CreateColoredTile(byte r, byte g, byte b)
    {
        int size = _tileSize;
        byte[] pixels = new byte[size * size * 4];

        for (int i = 0; i < size * size; i++)
        {
            pixels[(i * 4) + 0] = r;
            pixels[(i * 4) + 1] = g;
            pixels[(i * 4) + 2] = b;
            pixels[(i * 4) + 3] = 255;
        }

        var texture = new Texture2D(_graphicsDevice, size, size);
        texture.SetData(pixels);

        return texture;
    }
}
