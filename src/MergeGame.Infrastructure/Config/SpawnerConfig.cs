using System.Collections.Generic;

namespace MergeGame.Infrastructure.Config;

/// <summary>Configuration for a single spawner cell.</summary>
public sealed class SpawnerConfig
{
    /// <summary>Gets or sets the display name of this spawner.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets the description of this spawner.</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Gets or sets the image path for this spawner's tile.</summary>
    public string Image { get; set; } = string.Empty;

    /// <summary>Gets or sets the column index of the spawner cell.</summary>
    public int Column { get; set; }

    /// <summary>Gets or sets the row index of the spawner cell.</summary>
    public int Row { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of items this spawner may produce before being removed.
    /// Zero means unlimited.
    /// </summary>
    public int ItemLimit { get; set; }

    /// <summary>Gets or sets the list of items this spawner can produce, with their relative weights.</summary>
    public List<SpawnableItemConfig> SpawnableItems { get; set; } = [];
}
