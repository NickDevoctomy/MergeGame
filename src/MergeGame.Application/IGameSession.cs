using MergeGame.Domain;

namespace MergeGame.Application;

/// <summary>Represents the active game session, owning the grid and merge rule.</summary>
public interface IGameSession
{
    /// <summary>Gets the merge grid.</summary>
    public MergeGrid Grid { get; }

    /// <summary>Gets the merge rule in use for this session.</summary>
    public IMergeRule MergeRule { get; }
}
