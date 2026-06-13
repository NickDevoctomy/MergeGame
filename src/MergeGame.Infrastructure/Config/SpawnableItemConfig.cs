namespace MergeGame.Infrastructure.Config;

/// <summary>One entry in a spawner's probability table, linking an item name to its spawn weight.</summary>
public sealed class SpawnableItemConfig
{
    /// <summary>Gets or sets the name of the item to spawn (must match an entry in the <c>items</c> chain).</summary>
    public string ItemName { get; set; } = string.Empty;

    /// <summary>Gets or sets the relative spawn weight for this item.</summary>
    public int Weight { get; set; }
}
