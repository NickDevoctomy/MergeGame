using System.Collections.Generic;

using MergeGame.Application;
using MergeGame.Application.Handlers;
using MergeGame.Domain;
using MergeGame.Infrastructure.Config;
using MergeGame.Infrastructure.Random;
using MergeGame.Infrastructure.Tiles;

using Microsoft.Extensions.DependencyInjection;

namespace MergeGame.Game;

/// <summary>Builds and returns the application service provider.</summary>
internal static class CompositionRoot
{
    internal static ServiceProvider Build(GameConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);

        var services = new ServiceCollection();

        services.AddSingleton(config);

        MergeGrid grid = new MergeGrid(config.Grid.Columns, config.Grid.Rows);
        services.AddSingleton(grid);

        IMergeRule mergeRule = new StandardMergeRule(config.Tiles.MaxLevel);
        services.AddSingleton(mergeRule);

        services.AddSingleton<IRandomProvider, SystemRandomProvider>();
        services.AddSingleton<SpawnerService>();

        services.AddSingleton<IGameSession>(sp => new GameSession(
            sp.GetRequiredService<MergeGrid>(),
            sp.GetRequiredService<IMergeRule>()));

        services.AddSingleton<ITileGenerator>(new BmpTileGenerator(config.Tiles));

        services.AddTransient<ActivateSpawnerHandler>();
        services.AddTransient<MergeItemsHandler>();
        services.AddTransient<MoveItemHandler>();
        services.AddTransient<GetGameStateHandler>();

        return services.BuildServiceProvider();
    }

    internal static void PlaceSpawners(GameConfig config, IGameSession session)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(session);

        foreach (SpawnerConfig spawnerConfig in config.Spawners)
        {
            Dictionary<ItemLevel, int> weights = new Dictionary<ItemLevel, int>();

            foreach (KeyValuePair<int, int> kvp in spawnerConfig.Weights)
            {
                weights[new ItemLevel(kvp.Key)] = kvp.Value;
            }

            SpawnerDefinition definition = new SpawnerDefinition(weights);
            GridPosition position = new GridPosition(spawnerConfig.Column, spawnerConfig.Row);
            session.Grid.PlaceSpawner(position, definition);
        }
    }
}
