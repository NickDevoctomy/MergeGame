namespace MergeGame.Infrastructure.Tiles;

/// <summary>Raw RGBA pixel data for a generated tile texture.</summary>
public sealed class TileData
{
    /// <summary>Initializes a new instance of the <see cref="TileData"/> class.</summary>
    /// <param name="width">Tile width in pixels.</param>
    /// <param name="height">Tile height in pixels.</param>
    /// <param name="rgbaPixels">
    /// Raw RGBA byte array in row-major order. Length must equal <c>width * height * 4</c>.
    /// </param>
    public TileData(int width, int height, byte[] rgbaPixels)
    {
        ArgumentNullException.ThrowIfNull(rgbaPixels);

        if (width <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(width), "Width must be greater than zero.");
        }

        if (height <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(height), "Height must be greater than zero.");
        }

        int expected = width * height * 4;

        if (rgbaPixels.Length != expected)
        {
            throw new ArgumentException(
                $"Pixel array length {rgbaPixels.Length} does not match expected {expected} for {width}×{height} RGBA.",
                nameof(rgbaPixels));
        }

        Width = width;
        Height = height;
        RgbaPixels = rgbaPixels;
    }

    /// <summary>Gets the tile width in pixels.</summary>
    public int Width { get; }

    /// <summary>Gets the tile height in pixels.</summary>
    public int Height { get; }

    /// <summary>Gets the raw RGBA pixel data in row-major order.</summary>
    public byte[] RgbaPixels { get; }
}
