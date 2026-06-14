using System.Collections.Generic;

namespace MergeGame.Domain;

/// <summary>Defines the weighted spawn probabilities for a spawner cell, along with its identity and optional item limit.</summary>
public sealed class SpawnerDefinition
{
    /// <summary>Initializes a new instance of the <see cref="SpawnerDefinition"/> class.</summary>
    /// <param name="weights">
    /// Maps each spawnable <see cref="ItemDefinition"/> to its relative weight.
    /// The collection must be non-empty and the total weight must be greater than zero.
    /// </param>
    /// <param name="name">The display name of this spawner.</param>
    /// <param name="description">A short description of this spawner.</param>
    /// <param name="imagePath">Path to the image displayed on this spawner's tile.</param>
    /// <param name="itemLimit">
    /// Maximum number of items this spawner may produce before being removed from the grid.
    /// Zero means unlimited.
    /// </param>
    /// <param name="backgroundColor">Optional CSS hex colour string (e.g. <c>#FF8800</c>) drawn behind the tile image.</param>
    public SpawnerDefinition(
        IReadOnlyDictionary<ItemDefinition, int> weights,
        string name,
        string description,
        string imagePath,
        int itemLimit = 0,
        string? backgroundColor = null)
    {
        ArgumentNullException.ThrowIfNull(weights);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(description);
        ArgumentNullException.ThrowIfNull(imagePath);

        if (weights.Count == 0)
        {
            throw new ArgumentException("Weights must contain at least one entry.", nameof(weights));
        }

        int total = 0;
        foreach (int w in weights.Values)
        {
            total += w;
        }

        if (total <= 0)
        {
            throw new ArgumentException("Total weight must be greater than zero.", nameof(weights));
        }

        if (itemLimit < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(itemLimit), "Item limit must be zero or greater.");
        }

        Weights = weights;
        Name = name;
        Description = description;
        ImagePath = imagePath;
        ItemLimit = itemLimit;
        BackgroundColor = backgroundColor;
    }

    /// <summary>Gets the display name of this spawner.</summary>
    public string Name { get; }

    /// <summary>Gets the description of this spawner.</summary>
    public string Description { get; }

    /// <summary>Gets the image path for this spawner's tile.</summary>
    public string ImagePath { get; }

    /// <summary>Gets the optional background colour as a CSS hex string (e.g. <c>#FF8800</c>), drawn behind the tile image.</summary>
    public string? BackgroundColor { get; }

    /// <summary>Gets the maximum number of items this spawner may produce. Zero means unlimited.</summary>
    public int ItemLimit { get; }

    /// <summary>Gets the number of items this spawner has produced so far.</summary>
    public int SpawnCount { get; private set; }

    /// <summary>Gets a value indicating whether this spawner has reached its item limit and should be removed.</summary>
    public bool IsExhausted => ItemLimit > 0 && SpawnCount >= ItemLimit;

    /// <summary>Gets the spawn probability weights keyed by item definition.</summary>
    public IReadOnlyDictionary<ItemDefinition, int> Weights { get; }

    /// <summary>Records one item having been produced by this spawner, incrementing <see cref="SpawnCount"/>.</summary>
    public void RecordSpawn()
    {
        SpawnCount++;
    }
}
