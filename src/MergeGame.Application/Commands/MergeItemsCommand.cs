using MergeGame.Domain;

namespace MergeGame.Application.Commands;

/// <summary>Requests that the item at <see cref="Source"/> be merged into the item at <see cref="Target"/>.</summary>
public sealed record MergeItemsCommand(GridPosition Source, GridPosition Target);
