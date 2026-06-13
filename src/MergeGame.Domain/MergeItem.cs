namespace MergeGame.Domain;

/// <summary>An item placed on the merge grid.</summary>
public sealed class MergeItem
{
    /// <summary>Initializes a new instance of the <see cref="MergeItem"/> class.</summary>
    public MergeItem(ItemLevel level, GridPosition position)
    {
        Level = level;
        Position = position;
    }

    /// <summary>Gets the item's current level.</summary>
    public ItemLevel Level { get; }

    /// <summary>Gets the item's position on the grid.</summary>
    public GridPosition Position { get; }

    /// <summary>Returns a new item at a different grid position, preserving the level.</summary>
    public MergeItem WithPosition(GridPosition position)
    {
        return new MergeItem(Level, position);
    }
}
