namespace MergeGame.Domain;

/// <summary>An item placed on the merge grid.</summary>
public sealed class MergeItem
{
    /// <summary>Initializes a new instance of the <see cref="MergeItem"/> class.</summary>
    public MergeItem(ItemDefinition definition, GridPosition position)
    {
        ArgumentNullException.ThrowIfNull(definition);
        Definition = definition;
        Position = position;
    }

    /// <summary>Gets the item's definition (name, image, merge chain).</summary>
    public ItemDefinition Definition { get; }

    /// <summary>Gets the item's position on the grid.</summary>
    public GridPosition Position { get; }

    /// <summary>Returns a new item at a different grid position, preserving the definition.</summary>
    public MergeItem WithPosition(GridPosition position)
    {
        return new MergeItem(Definition, position);
    }
}
