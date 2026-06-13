using MergeGame.Domain;

namespace MergeGame.Application;

/// <summary>The default game session implementation.</summary>
public sealed class GameSession : IGameSession
{
    /// <summary>Initializes a new instance of the <see cref="GameSession"/> class.</summary>
    public GameSession(MergeGrid grid, IMergeRule mergeRule)
    {
        ArgumentNullException.ThrowIfNull(grid);
        ArgumentNullException.ThrowIfNull(mergeRule);

        Grid = grid;
        MergeRule = mergeRule;
    }

    /// <inheritdoc/>
    public MergeGrid Grid { get; }

    /// <inheritdoc/>
    public IMergeRule MergeRule { get; }
}
