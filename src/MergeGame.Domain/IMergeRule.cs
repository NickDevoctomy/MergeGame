namespace MergeGame.Domain;

/// <summary>Defines the rule that governs whether and how two items can be merged.</summary>
public interface IMergeRule
{
    /// <summary>Returns <see langword="true"/> when items <paramref name="a"/> and <paramref name="b"/> are eligible to merge.</summary>
    /// <param name="a">The first item.</param>
    /// <param name="b">The second item.</param>
    /// <returns><see langword="true"/> if the items can be merged; otherwise <see langword="false"/>.</returns>
    public bool CanMerge(MergeItem a, MergeItem b);

    /// <summary>Merges items <paramref name="a"/> and <paramref name="b"/>, placing the result at <paramref name="targetPosition"/>.</summary>
    /// <param name="a">The first item.</param>
    /// <param name="b">The second item.</param>
    /// <param name="targetPosition">The grid position the merged item will occupy.</param>
    /// <returns>The newly produced <see cref="MergeItem"/>.</returns>
    public MergeItem Merge(MergeItem a, MergeItem b, GridPosition targetPosition);
}
