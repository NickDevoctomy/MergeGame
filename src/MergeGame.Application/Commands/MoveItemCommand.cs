using MergeGame.Domain;

namespace MergeGame.Application.Commands;

/// <summary>Requests that the item at <see cref="Source"/> be moved to the empty cell at <see cref="Target"/>.</summary>
public sealed record MoveItemCommand(GridPosition Source, GridPosition Target);
