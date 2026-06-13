namespace MergeGame.Domain;

/// <summary>
/// The standard merge rule: two items of the same level merge into one item of the next level.
/// Items at <see cref="MaxLevel"/> cannot merge further.
/// </summary>
public sealed class StandardMergeRule : IMergeRule
{
    /// <summary>Initializes a new instance of the <see cref="StandardMergeRule"/> class.</summary>
    /// <param name="maxLevel">The highest level an item can reach; items at this level cannot merge.</param>
    public StandardMergeRule(int maxLevel)
    {
        if (maxLevel <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxLevel), "Max level must be greater than zero.");
        }

        MaxLevel = maxLevel;
    }

    /// <summary>Gets the maximum item level allowed by this rule.</summary>
    public int MaxLevel { get; }

    /// <inheritdoc/>
    public bool CanMerge(MergeItem a, MergeItem b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        return a.Level == b.Level && a.Level.Value < MaxLevel;
    }

    /// <inheritdoc/>
    public MergeItem Merge(MergeItem a, MergeItem b, GridPosition targetPosition)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        if (!CanMerge(a, b))
        {
            throw new InvalidOperationException(
                $"Cannot merge items with levels {a.Level.Value} and {b.Level.Value} (max level: {MaxLevel}).");
        }

        return new MergeItem(a.Level.Next(), targetPosition);
    }
}
