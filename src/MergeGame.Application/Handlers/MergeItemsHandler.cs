using MergeGame.Application.Commands;
using MergeGame.Application.Results;
using MergeGame.Domain;

namespace MergeGame.Application.Handlers;

/// <summary>Handles <see cref="MergeItemsCommand"/> by delegating to the grid and the active merge rule.</summary>
public sealed class MergeItemsHandler
{
    private readonly IGameSession _session;

    /// <summary>Initializes a new instance of the <see cref="MergeItemsHandler"/> class.</summary>
    public MergeItemsHandler(IGameSession session)
    {
        ArgumentNullException.ThrowIfNull(session);
        _session = session;
    }

    /// <summary>Executes the command and returns the outcome.</summary>
    public MergeItemsResult Handle(MergeItemsCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        MergeResult result = _session.Grid.TryMerge(command.Source, command.Target, _session.MergeRule);

        return result switch
        {
            MergeResult.Success s => new MergeItemsResult.Success(s.ProducedItem),
            MergeResult.Failure f => new MergeItemsResult.Failed(f.Reason),
            _ => new MergeItemsResult.Failed("Unexpected merge result."),
        };
    }
}
