using System;

namespace MergeGame.Domain;

/// <summary>Defines a single item type within a named merge chain.</summary>
public sealed class ItemDefinition : IEquatable<ItemDefinition>
{
    /// <summary>Initializes a new instance of the <see cref="ItemDefinition"/> class.</summary>
    /// <param name="name">The unique name of this item type.</param>
    /// <param name="description">A short description shown to the player.</param>
    /// <param name="imagePath">Relative path to the PNG image asset for this item.</param>
    /// <param name="product">The item produced when two of this type are merged, or <see langword="null"/> if this is the final tier.</param>
    public ItemDefinition(string name, string description, string imagePath, ItemDefinition? product)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(imagePath);

        Name = name;
        Description = description ?? string.Empty;
        ImagePath = imagePath;
        Product = product;
    }

    /// <summary>Gets the unique name of this item type.</summary>
    public string Name { get; }

    /// <summary>Gets a short description of this item type.</summary>
    public string Description { get; }

    /// <summary>Gets the relative path to this item's PNG image asset.</summary>
    public string ImagePath { get; }

    /// <summary>Gets the item produced when two of this item are merged, or <see langword="null"/> if this is the final tier.</summary>
    public ItemDefinition? Product { get; }

    /// <inheritdoc/>
    public bool Equals(ItemDefinition? other)
    {
        return other is not null && string.Equals(Name, other.Name, StringComparison.Ordinal);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return Equals(obj as ItemDefinition);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Name.GetHashCode(StringComparison.Ordinal);
    }

    /// <summary>Returns the name of this item definition.</summary>
    public override string ToString()
    {
        return Name;
    }
}
