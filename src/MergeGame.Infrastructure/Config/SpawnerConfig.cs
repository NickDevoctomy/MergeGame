using System.Collections.Generic;

namespace MergeGame.Infrastructure.Config;

/// <summary>Configuration for a single spawner cell.</summary>
public sealed class SpawnerConfig
{
    /// <summary>Gets or sets the column index of the spawner cell.</summary>
    public int Column { get; set; }

    /// <summary>Gets or sets the row index of the spawner cell.</summary>
    public int Row { get; set; }

    /// <summary>Gets or sets the list of items this spawner can produce, with their relative weights.</summary>
    public List<SpawnableItemConfig> SpawnableItems { get; set; } = [];
}
