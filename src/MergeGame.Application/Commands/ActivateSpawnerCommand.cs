using MergeGame.Domain;

namespace MergeGame.Application.Commands;

/// <summary>Activates the spawner at the given grid position, placing a new item in a random empty cell.</summary>
public sealed record ActivateSpawnerCommand(GridPosition SpawnerPosition);
