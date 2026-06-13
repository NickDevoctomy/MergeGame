using System.Collections.Generic;

using MergeGame.Application.Dtos;
using MergeGame.Application.Queries;
using MergeGame.Domain;

namespace MergeGame.Application.Handlers;

/// <summary>Maps the current <see cref="MergeGrid"/> state into a <see cref="GameStateSnapshot"/> DTO.</summary>
public sealed class GetGameStateHandler
{
    private readonly IGameSession _session;

    /// <summary>Initializes a new instance of the <see cref="GetGameStateHandler"/> class.</summary>
    public GetGameStateHandler(IGameSession session)
    {
        ArgumentNullException.ThrowIfNull(session);
        _session = session;
    }

    /// <summary>Executes the query and returns the current snapshot.</summary>
    public GameStateSnapshot Handle(GetGameStateQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        var cells = new List<CellStateDto>(_session.Grid.Columns * _session.Grid.Rows);

        for (int c = 0; c < _session.Grid.Columns; c++)
        {
            for (int r = 0; r < _session.Grid.Rows; r++)
            {
                GridPosition position = new GridPosition(c, r);
                CellContent cell = _session.Grid.GetCell(position);

                CellStateDto dto = cell switch
                {
                    CellContent.Empty => new CellStateDto(position, null, false, true),
                    CellContent.Item item => new CellStateDto(position, item.MergeItem.Level.Value, false, false),
                    CellContent.Spawner => new CellStateDto(position, null, true, false),
                    _ => new CellStateDto(position, null, false, true),
                };

                cells.Add(dto);
            }
        }

        return new GameStateSnapshot(cells, _session.Grid.Columns, _session.Grid.Rows);
    }
}
