using System.Collections.Generic;

namespace MergeGame.Infrastructure.Config;

/// <summary>Root configuration object for the entire game, loaded from <c>game.json</c>.</summary>
public sealed class GameConfig
{
    /// <summary>Gets or sets the grid dimensions.</summary>
    public GridConfig Grid { get; set; } = new GridConfig();

    /// <summary>Gets or sets the list of spawner cells to place on the grid at startup.</summary>
    public List<SpawnerConfig> Spawners { get; set; } = [];

    /// <summary>Gets or sets tile visual settings.</summary>
    public TileConfig Tiles { get; set; } = new TileConfig();

    /// <summary>Gets or sets sound settings.</summary>
    public SoundConfig Sounds { get; set; } = new SoundConfig();
}
