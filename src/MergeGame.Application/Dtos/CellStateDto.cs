using MergeGame.Domain;

namespace MergeGame.Application.Dtos;

/// <summary>The state of a single grid cell, used in a <see cref="GameStateSnapshot"/>.</summary>
public sealed record CellStateDto(GridPosition Position, string? ItemName, string? ImagePath, bool IsSpawner, bool IsEmpty, string? BackgroundColor);
