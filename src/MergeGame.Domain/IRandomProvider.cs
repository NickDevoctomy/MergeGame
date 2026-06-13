namespace MergeGame.Domain;

/// <summary>Provides random integers; injectable for deterministic testing.</summary>
public interface IRandomProvider
{
    /// <summary>Returns a random integer in the range [<paramref name="minInclusive"/>, <paramref name="maxExclusive"/>).</summary>
    /// <param name="minInclusive">The inclusive lower bound.</param>
    /// <param name="maxExclusive">The exclusive upper bound.</param>
    /// <returns>A random integer within the specified range.</returns>
    public int GetNext(int minInclusive, int maxExclusive);
}
