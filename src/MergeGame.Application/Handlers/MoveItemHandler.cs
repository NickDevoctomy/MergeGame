using MergeGame.Application.Commands;
using MergeGame.Application.Results;
using MergeGame.Domain;

namespace MergeGame.Application.Handlers;

/// <summary>Handles <see cref="MoveItemCommand"/> by moving an item to an empty cell on the grid.</summary>
public sealed class MoveItemHandler
{
    private readonly IGameSession _session;

    /// <summary>Initializes a new instance of the <see cref="MoveItemHandler"/> class.</summary>
    public MoveItemHandler(IGameSession session)
    {
        ArgumentNullException.ThrowIfNull(session);
        _session = session;
    }

    /// <summary>Executes the command and returns the outcome.</summary>
    public MoveItemResult Handle(MoveItemCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        MoveResult result = _session.Grid.MoveItem(command.Source, command.Target);

        return result switch
        {
            MoveResult.Success s => new MoveItemResult.Success(s.MovedItem),
            MoveResult.Failure f => new MoveItemResult.Failed(f.Reason),
            _ => new MoveItemResult.Failed("Unexpected move result."),
        };
    }
}
