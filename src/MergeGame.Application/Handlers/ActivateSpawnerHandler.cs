using System.Collections.Generic;

using MergeGame.Application.Commands;
using MergeGame.Application.Results;
using MergeGame.Domain;

namespace MergeGame.Application.Handlers;

/// <summary>Handles <see cref="ActivateSpawnerCommand"/> by spawning a new item into a random empty cell.</summary>
public sealed class ActivateSpawnerHandler
{
    private readonly IGameSession _session;
    private readonly SpawnerService _spawnerService;
    private readonly IRandomProvider _randomProvider;

    /// <summary>Initializes a new instance of the <see cref="ActivateSpawnerHandler"/> class.</summary>
    public ActivateSpawnerHandler(
        IGameSession session,
        SpawnerService spawnerService,
        IRandomProvider randomProvider)
    {
        ArgumentNullException.ThrowIfNull(session);
        ArgumentNullException.ThrowIfNull(spawnerService);
        ArgumentNullException.ThrowIfNull(randomProvider);

        _session = session;
        _spawnerService = spawnerService;
        _randomProvider = randomProvider;
    }

    /// <summary>Executes the command and returns the outcome.</summary>
    public ActivateSpawnerResult Handle(ActivateSpawnerCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        CellContent cell = _session.Grid.GetCell(command.SpawnerPosition);

        if (cell is not CellContent.Spawner spawnerCell)
        {
            return new ActivateSpawnerResult.NotASpawner();
        }

        IReadOnlyList<GridPosition> emptyCells = _session.Grid.FindEmptyCells();

        if (emptyCells.Count == 0)
        {
            return new ActivateSpawnerResult.GridFull();
        }

        int index = _randomProvider.GetNext(0, emptyCells.Count);
        GridPosition targetPosition = emptyCells[index];

        ItemDefinition definition = SpawnerService.SpawnItem(spawnerCell.Definition, _randomProvider);
        MergeItem item = new MergeItem(definition, targetPosition);

        _session.Grid.PlaceItem(item);

        return new ActivateSpawnerResult.Success(item);
    }
}
