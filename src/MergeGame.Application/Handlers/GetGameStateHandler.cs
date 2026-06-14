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
                    CellContent.Empty => new CellStateDto(position, null, null, false, true, null),
                    CellContent.Item item => new CellStateDto(position, item.MergeItem.Definition.Name, item.MergeItem.Definition.ImagePath, false, false, item.MergeItem.Definition.BackgroundColor),
                    CellContent.Spawner s => new CellStateDto(position, s.Definition.Name, s.Definition.ImagePath, true, false, s.Definition.BackgroundColor),
                    _ => new CellStateDto(position, null, null, false, true, null),
                };

                cells.Add(dto);
            }
        }

        return new GameStateSnapshot(cells, _session.Grid.Columns, _session.Grid.Rows);
    }
}
