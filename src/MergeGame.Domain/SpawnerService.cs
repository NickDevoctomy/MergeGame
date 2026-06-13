using System.Collections.Generic;

namespace MergeGame.Domain;

/// <summary>Selects a random <see cref="ItemDefinition"/> from a <see cref="SpawnerDefinition"/> using weighted probability.</summary>
public sealed class SpawnerService
{
    /// <summary>
    /// Returns a randomly chosen <see cref="ItemDefinition"/> from <paramref name="definition"/>,
    /// weighted according to its probability table.
    /// </summary>
    public static ItemDefinition SpawnItem(SpawnerDefinition definition, IRandomProvider randomProvider)
    {
        ArgumentNullException.ThrowIfNull(definition);
        ArgumentNullException.ThrowIfNull(randomProvider);

        int total = 0;
        foreach (int w in definition.Weights.Values)
        {
            total += w;
        }

        int roll = randomProvider.GetNext(0, total);

        int cumulative = 0;
        ItemDefinition? last = null;
        foreach (KeyValuePair<ItemDefinition, int> entry in definition.Weights)
        {
            if (entry.Value <= 0)
            {
                continue;
            }

            cumulative += entry.Value;
            last = entry.Key;

            if (roll < cumulative)
            {
                return entry.Key;
            }
        }

        return last!;
    }
}
