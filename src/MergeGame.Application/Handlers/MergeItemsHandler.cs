using MergeGame.Application.Commands;
using MergeGame.Application.Results;
using MergeGame.Domain;

namespace MergeGame.Application.Handlers;

/// <summary>Handles <see cref="MergeItemsCommand"/> by delegating to the grid and the active merge rule.</summary>
public sealed class MergeItemsHandler
{
    private readonly IGameSession _session;
    private readonly ISoundService _soundService;

    /// <summary>Initializes a new instance of the <see cref="MergeItemsHandler"/> class.</summary>
    public MergeItemsHandler(IGameSession session, ISoundService soundService)
    {
        ArgumentNullException.ThrowIfNull(session);
        ArgumentNullException.ThrowIfNull(soundService);
        _session = session;
        _soundService = soundService;
    }

    /// <summary>Executes the command and returns the outcome.</summary>
    public MergeItemsResult Handle(MergeItemsCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        MergeResult result = _session.Grid.TryMerge(command.Source, command.Target, _session.MergeRule);

        switch (result)
        {
            case MergeResult.Success s:
                _soundService.PlaySound("merge");
                return new MergeItemsResult.Success(s.ProducedItem);

            case MergeResult.Failure f:
                _soundService.PlaySound("invalid");
                return new MergeItemsResult.Failed(f.Reason);

            default:
                _soundService.PlaySound("invalid");
                return new MergeItemsResult.Failed("Unexpected merge result.");
        }
    }
}
