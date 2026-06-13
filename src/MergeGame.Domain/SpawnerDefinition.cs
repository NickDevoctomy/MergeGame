using System.Collections.Generic;

namespace MergeGame.Domain;

/// <summary>Defines the weighted spawn probabilities for a spawner cell.</summary>
public sealed class SpawnerDefinition
{
    /// <summary>Initializes a new instance of the <see cref="SpawnerDefinition"/> class.</summary>
    /// <param name="weights">
    /// Maps each spawnable <see cref="ItemLevel"/> to its relative weight.
    /// The collection must be non-empty and the total weight must be greater than zero.
    /// </param>
    public SpawnerDefinition(IReadOnlyDictionary<ItemLevel, int> weights)
    {
        ArgumentNullException.ThrowIfNull(weights);

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

        Weights = weights;
    }

    /// <summary>Gets the spawn probability weights keyed by item level.</summary>
    public IReadOnlyDictionary<ItemLevel, int> Weights { get; }
}
