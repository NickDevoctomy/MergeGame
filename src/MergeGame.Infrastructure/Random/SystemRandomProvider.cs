using MergeGame.Domain;

namespace MergeGame.Infrastructure.Random;

/// <summary><see cref="IRandomProvider"/> implementation backed by <see cref="System.Random"/>.</summary>
public sealed class SystemRandomProvider : IRandomProvider
{
    private readonly System.Random _random = new System.Random();

    /// <inheritdoc/>
    public int GetNext(int minInclusive, int maxExclusive)
    {
        return _random.Next(minInclusive, maxExclusive);
    }
}
