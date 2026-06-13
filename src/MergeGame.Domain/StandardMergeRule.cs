namespace MergeGame.Domain;

/// <summary>
/// The standard merge rule: two items sharing the same <see cref="ItemDefinition"/> merge
/// into one item of the definition's <see cref="ItemDefinition.Product"/> type.
/// Items whose definition has no product cannot merge further.
/// </summary>
public sealed class StandardMergeRule : IMergeRule
{
    /// <inheritdoc/>
    public bool CanMerge(MergeItem a, MergeItem b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        return a.Definition.Equals(b.Definition) && a.Definition.Product is not null;
    }

    /// <inheritdoc/>
    public MergeItem Merge(MergeItem a, MergeItem b, GridPosition targetPosition)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        if (!CanMerge(a, b))
        {
            throw new InvalidOperationException(
                $"Cannot merge '{a.Definition.Name}' with '{b.Definition.Name}'.");
        }

        return new MergeItem(a.Definition.Product!, targetPosition);
    }
}
