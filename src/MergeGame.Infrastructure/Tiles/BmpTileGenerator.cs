using System.Collections.Generic;
using System.Globalization;

using MergeGame.Infrastructure.Config;

namespace MergeGame.Infrastructure.Tiles;

/// <summary>
/// Generates coloured RGBA tiles at runtime.
/// Each item tile has a solid colour background (per config or auto-HSL) with the level number
/// centred using an embedded 5x7 pixel bitmap font scaled to fit the tile.
/// </summary>
public sealed class BmpTileGenerator : ITileGenerator
{
    private const int GlyphWidth = 5;
    private const int GlyphHeight = 7;
    private const int BorderWidth = 2;

    private static readonly Dictionary<char, byte[]> FontGlyphs = new Dictionary<char, byte[]>
    {
        ['0'] = new byte[] { 0x0E, 0x11, 0x13, 0x15, 0x19, 0x11, 0x0E },
        ['1'] = new byte[] { 0x04, 0x0C, 0x04, 0x04, 0x04, 0x04, 0x0E },
        ['2'] = new byte[] { 0x0E, 0x11, 0x01, 0x06, 0x08, 0x10, 0x1F },
        ['3'] = new byte[] { 0x0E, 0x11, 0x01, 0x06, 0x01, 0x11, 0x0E },
        ['4'] = new byte[] { 0x02, 0x06, 0x0A, 0x12, 0x1F, 0x02, 0x02 },
        ['5'] = new byte[] { 0x1F, 0x10, 0x1E, 0x01, 0x01, 0x11, 0x0E },
        ['6'] = new byte[] { 0x06, 0x08, 0x10, 0x1E, 0x11, 0x11, 0x0E },
        ['7'] = new byte[] { 0x1F, 0x01, 0x02, 0x04, 0x08, 0x08, 0x08 },
        ['8'] = new byte[] { 0x0E, 0x11, 0x11, 0x0E, 0x11, 0x11, 0x0E },
        ['9'] = new byte[] { 0x0E, 0x11, 0x11, 0x0F, 0x01, 0x02, 0x0C },
    };

    private readonly TileConfig _config;

    /// <summary>Initializes a new instance of the <see cref="BmpTileGenerator"/> class.</summary>
    /// <param name="config">The tile visual configuration.</param>
    public BmpTileGenerator(TileConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);
        _config = config;
    }

    /// <inheritdoc/>
    public TileData GenerateTile(int level)
    {
        if (level <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(level), "Level must be greater than zero.");
        }

        (byte r, byte g, byte b) = GetLevelColor(level);

        byte[] pixels = CreateSolidBackground(_config.TileSize, r, g, b);
        DrawBorder(pixels, _config.TileSize, Darken(r), Darken(g), Darken(b));
        DrawNumber(pixels, _config.TileSize, level);

        return new TileData(_config.TileSize, _config.TileSize, pixels);
    }

    /// <inheritdoc/>
    public TileData GenerateEmptyTile()
    {
        byte[] pixels = CreateSolidBackground(_config.TileSize, 0x2A, 0x2A, 0x2A);
        DrawBorder(pixels, _config.TileSize, 0x1A, 0x1A, 0x1A);
        return new TileData(_config.TileSize, _config.TileSize, pixels);
    }

    /// <inheritdoc/>
    public TileData GenerateSpawnerTile()
    {
        byte[] pixels = CreateSolidBackground(_config.TileSize, 0x1A, 0x4A, 0x1A);
        DrawBorder(pixels, _config.TileSize, 0x0A, 0x2A, 0x0A);
        DrawPlus(pixels, _config.TileSize);
        return new TileData(_config.TileSize, _config.TileSize, pixels);
    }

    /// <summary>
    /// Parses a hex colour string of the form <c>#RRGGBB</c> into individual byte channels.
    /// Exposed as internal for unit testing.
    /// </summary>
    /// <param name="hex">The hex colour string.</param>
    /// <returns>A tuple of (R, G, B) byte values.</returns>
    internal static (byte R, byte G, byte B) ParseHexColor(string hex)
    {
        string cleaned = hex.TrimStart('#');
        byte r = Convert.ToByte(cleaned[0..2], 16);
        byte g = Convert.ToByte(cleaned[2..4], 16);
        byte b = Convert.ToByte(cleaned[4..6], 16);

        return (r, g, b);
    }

    private static (byte R, byte G, byte B) HslToRgb(float h, float s, float l)
    {
        float c = (1f - MathF.Abs((2f * l) - 1f)) * s;
        float x = c * (1f - MathF.Abs(((h / 60f) % 2f) - 1f));
        float m = l - (c / 2f);

        float rf;
        float gf;
        float bf;

        if (h < 60f)
        {
            rf = c;
            gf = x;
            bf = 0f;
        }
        else if (h < 120f)
        {
            rf = x;
            gf = c;
            bf = 0f;
        }
        else if (h < 180f)
        {
            rf = 0f;
            gf = c;
            bf = x;
        }
        else if (h < 240f)
        {
            rf = 0f;
            gf = x;
            bf = c;
        }
        else if (h < 300f)
        {
            rf = x;
            gf = 0f;
            bf = c;
        }
        else
        {
            rf = c;
            gf = 0f;
            bf = x;
        }

        return (
            (byte)((rf + m) * 255f),
            (byte)((gf + m) * 255f),
            (byte)((bf + m) * 255f));
    }

    private static byte Darken(byte channel)
    {
        return (byte)(channel * 0.65f);
    }

    private static byte[] CreateSolidBackground(int size, byte r, byte g, byte b)
    {
        byte[] pixels = new byte[size * size * 4];

        for (int i = 0; i < size * size; i++)
        {
            pixels[(i * 4) + 0] = r;
            pixels[(i * 4) + 1] = g;
            pixels[(i * 4) + 2] = b;
            pixels[(i * 4) + 3] = 255;
        }

        return pixels;
    }

    private static void DrawBorder(byte[] pixels, int size, byte r, byte g, byte b)
    {
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if (x < BorderWidth || x >= size - BorderWidth
                    || y < BorderWidth || y >= size - BorderWidth)
                {
                    SetPixel(pixels, size, x, y, r, g, b, 255);
                }
            }
        }
    }

    private static void DrawNumber(byte[] pixels, int tileSize, int level)
    {
        int scale = Math.Max(1, tileSize / 16);
        string text = level.ToString(CultureInfo.InvariantCulture);
        int charW = GlyphWidth * scale;
        int spacing = scale;
        int totalWidth = (text.Length * charW) + ((text.Length - 1) * spacing);
        int totalHeight = GlyphHeight * scale;

        int startX = (tileSize - totalWidth) / 2;
        int startY = (tileSize - totalHeight) / 2;
        int charX = startX;

        foreach (char c in text)
        {
            if (FontGlyphs.TryGetValue(c, out byte[]? glyph))
            {
                DrawGlyph(pixels, tileSize, glyph, charX, startY, scale);
            }

            charX += charW + spacing;
        }
    }

    private static void DrawGlyph(byte[] pixels, int tileSize, byte[] glyph, int startX, int startY, int scale)
    {
        for (int row = 0; row < glyph.Length; row++)
        {
            for (int col = 0; col < GlyphWidth; col++)
            {
                bool on = ((glyph[row] >> (GlyphWidth - 1 - col)) & 1) == 1;

                if (!on)
                {
                    continue;
                }

                for (int sy = 0; sy < scale; sy++)
                {
                    for (int sx = 0; sx < scale; sx++)
                    {
                        int px = startX + (col * scale) + sx;
                        int py = startY + (row * scale) + sy;

                        if (px >= 0 && px < tileSize && py >= 0 && py < tileSize)
                        {
                            SetPixel(pixels, tileSize, px, py, 255, 255, 255, 255);
                        }
                    }
                }
            }
        }
    }

    private static void DrawPlus(byte[] pixels, int tileSize)
    {
        int scale = Math.Max(1, tileSize / 16);
        int thickness = scale * 2;
        int cx = tileSize / 2;
        int cy = tileSize / 2;
        int arm = tileSize / 4;

        for (int y = cy - arm; y <= cy + arm; y++)
        {
            for (int x = cx - (thickness / 2); x <= cx + (thickness / 2); x++)
            {
                if (x >= 0 && x < tileSize && y >= 0 && y < tileSize)
                {
                    SetPixel(pixels, tileSize, x, y, 255, 255, 255, 255);
                }
            }
        }

        for (int x = cx - arm; x <= cx + arm; x++)
        {
            for (int y = cy - (thickness / 2); y <= cy + (thickness / 2); y++)
            {
                if (x >= 0 && x < tileSize && y >= 0 && y < tileSize)
                {
                    SetPixel(pixels, tileSize, x, y, 255, 255, 255, 255);
                }
            }
        }
    }

    private static void SetPixel(byte[] pixels, int tileSize, int x, int y, byte r, byte g, byte b, byte a)
    {
        int idx = ((y * tileSize) + x) * 4;
        pixels[idx + 0] = r;
        pixels[idx + 1] = g;
        pixels[idx + 2] = b;
        pixels[idx + 3] = a;
    }

    private (byte R, byte G, byte B) GetLevelColor(int level)
    {
        if (_config.LevelColors.TryGetValue(level, out string? hex))
        {
            return ParseHexColor(hex);
        }

        float hue = ((level - 1) * 36f) % 360f;

        return HslToRgb(hue, 0.7f, 0.5f);
    }
}
