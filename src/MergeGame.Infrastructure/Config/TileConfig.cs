using System.Collections.Generic;

namespace MergeGame.Infrastructure.Config;

/// <summary>Tile visual configuration loaded from configuration.</summary>
public sealed class TileConfig
{
    /// <summary>Gets or sets the pixel size of each tile (width and height).</summary>
    public int TileSize { get; set; }

    /// <summary>Gets or sets the highest item level allowed. Items at this level cannot merge further.</summary>
    public int MaxLevel { get; set; }

    /// <summary>
    /// Gets or sets the background colour for each level.
    /// Keys are level numbers; values are hex colour strings in <c>#RRGGBB</c> format.
    /// Levels not present here receive an auto-generated colour.
    /// </summary>
    public Dictionary<int, string> LevelColors { get; set; } = [];
}
