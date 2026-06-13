using System.Collections.Generic;

namespace MergeGame.Application.Dtos;

/// <summary>A point-in-time snapshot of the full grid state.</summary>
public sealed record GameStateSnapshot(IReadOnlyList<CellStateDto> Cells, int Columns, int Rows);
