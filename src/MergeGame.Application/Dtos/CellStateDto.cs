using MergeGame.Domain;

namespace MergeGame.Application.Dtos;

/// <summary>The state of a single grid cell, used in a <see cref="GameStateSnapshot"/>.</summary>
public sealed record CellStateDto(GridPosition Position, int? Level, bool IsSpawner, bool IsEmpty);
