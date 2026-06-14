using System.Collections.Generic;

using MergeGame.Application;
using MergeGame.Application.Handlers;
using MergeGame.Domain;
using MergeGame.Infrastructure.Config;
using MergeGame.Infrastructure.Random;

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

        IMergeRule mergeRule = new StandardMergeRule();
        services.AddSingleton(mergeRule);

        services.AddSingleton<IRandomProvider, SystemRandomProvider>();
        services.AddSingleton<SpawnerService>();

        ISoundService soundService = config.Sounds.Enabled
            ? new SoundService(config.Sounds)
            : new NullSoundService();
        services.AddSingleton(soundService);

        services.AddSingleton<IGameSession>(sp => new GameSession(
            sp.GetRequiredService<MergeGrid>(),
            sp.GetRequiredService<IMergeRule>()));

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

        Dictionary<string, ItemDefinition> itemsByName = BuildItemDictionary(config.Items);

        foreach (SpawnerConfig spawnerConfig in config.Spawners)
        {
            var weights = new Dictionary<ItemDefinition, int>();

            foreach (SpawnableItemConfig si in spawnerConfig.SpawnableItems)
            {
                if (itemsByName.TryGetValue(si.ItemName, out ItemDefinition? def))
                {
                    weights[def] = si.Weight;
                }
            }

            SpawnerDefinition definition = new SpawnerDefinition(
                weights,
                spawnerConfig.Name,
                spawnerConfig.Description,
                spawnerConfig.Image,
                spawnerConfig.ItemLimit,
                spawnerConfig.BackgroundColor);
            GridPosition position = new GridPosition(spawnerConfig.Column, spawnerConfig.Row);
            session.Grid.PlaceSpawner(position, definition);
        }
    }

    private static Dictionary<string, ItemDefinition> BuildItemDictionary(List<ItemChainConfig> items)
    {
        var result = new Dictionary<string, ItemDefinition>(System.StringComparer.Ordinal);

        foreach (ItemChainConfig root in items)
        {
            BuildDefinition(root, result);
        }

        return result;
    }

    private static ItemDefinition BuildDefinition(ItemChainConfig config, Dictionary<string, ItemDefinition> registry)
    {
        ItemDefinition? product = config.Product is not null
            ? BuildDefinition(config.Product, registry)
            : null;

        var def = new ItemDefinition(config.Name, config.Description, config.Image, product, config.BackgroundColor);
        registry[config.Name] = def;

        return def;
    }
}
