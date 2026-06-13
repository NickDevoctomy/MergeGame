namespace MergeGame.Domain;

/// <summary>The numeric level of a merge item. Must be greater than zero.</summary>
public readonly record struct ItemLevel
{
    /// <summary>Initializes a new instance of the <see cref="ItemLevel"/> struct.</summary>
    /// <param name="value">The level value; must be greater than zero.</param>
    public ItemLevel(int value)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Item level must be greater than zero.");
        }

        Value = value;
    }

    /// <summary>Gets the numeric level value.</summary>
    public int Value { get; init; }

    /// <summary>Returns the next level up from this one.</summary>
    public ItemLevel Next()
    {
        return new ItemLevel(Value + 1);
    }
}
