namespace MergeGame.Infrastructure.Config;

/// <summary>One node in a named merge chain, loaded from configuration.</summary>
public sealed class ItemChainConfig
{
    /// <summary>Gets or sets the unique name of this item type.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets a short description of this item type.</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Gets or sets the relative path to this item's PNG image asset.</summary>
    public string Image { get; set; } = string.Empty;

    /// <summary>Gets or sets the item produced when two of this type are merged, or <see langword="null"/> if this is the final tier.</summary>
    public ItemChainConfig? Product { get; set; }
}
