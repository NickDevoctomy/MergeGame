using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace MergeGame.Infrastructure.Config;

/// <summary>Loads <see cref="GameConfig"/> from a JSON file on disk.</summary>
public sealed class JsonConfigLoader : IConfigLoader
{
    private static readonly JsonSerializerOptions DeserializeOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
    };

    /// <summary>
    /// Deserialises and validates a JSON string into a <see cref="GameConfig"/>.
    /// Exposed as public static so unit tests can call it directly without writing to disk.
    /// </summary>
    /// <param name="json">The JSON string to parse.</param>
    /// <returns>The validated <see cref="GameConfig"/>.</returns>
    public static GameConfig ParseConfig(string json)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(json);

        GameConfig? config;

        try
        {
            config = JsonSerializer.Deserialize<GameConfig>(json, DeserializeOptions);
        }
        catch (JsonException ex)
        {
            throw new ConfigurationException("Configuration JSON is malformed.", ex);
        }

        if (config is null)
        {
            throw new ConfigurationException("Configuration JSON deserialised to null.");
        }

        Validate(config);

        return config;
    }

    /// <inheritdoc/>
    public GameConfig Load()
    {
        const string Path = "game.json";

        if (!File.Exists(Path))
        {
            throw new ConfigurationException($"Configuration file '{Path}' was not found in the working directory.");
        }

        string json;

        try
        {
            json = File.ReadAllText(Path);
        }
        catch (IOException ex)
        {
            throw new ConfigurationException($"Failed to read configuration file '{Path}'.", ex);
        }

        return ParseConfig(json);
    }

    private static void Validate(GameConfig config)
    {
        if (config.Grid.Columns <= 0)
        {
            throw new ConfigurationException("grid.columns must be greater than zero.");
        }

        if (config.Grid.Rows <= 0)
        {
            throw new ConfigurationException("grid.rows must be greater than zero.");
        }

        if (config.TileSize <= 0)
        {
            throw new ConfigurationException("tileSize must be greater than zero.");
        }

        if (config.Items.Count == 0)
        {
            throw new ConfigurationException("items must contain at least one entry.");
        }

        HashSet<string> allNames = new HashSet<string>(System.StringComparer.Ordinal);
        foreach (ItemChainConfig root in config.Items)
        {
            CollectAndValidateChain(root, allNames);
        }

        foreach (SpawnerConfig spawner in config.Spawners)
        {
            if (string.IsNullOrWhiteSpace(spawner.Name))
            {
                throw new ConfigurationException(
                    $"Spawner at ({spawner.Column},{spawner.Row}) must have a non-empty name.");
            }

            if (string.IsNullOrWhiteSpace(spawner.Image))
            {
                throw new ConfigurationException(
                    $"Spawner '{spawner.Name}' must have a non-empty image path.");
            }

            if (spawner.ItemLimit < 0)
            {
                throw new ConfigurationException(
                    $"Spawner '{spawner.Name}' itemLimit must be zero or greater.");
            }

            if (spawner.Column < 0 || spawner.Column >= config.Grid.Columns)
            {
                throw new ConfigurationException(
                    $"Spawner column {spawner.Column} is outside the grid width {config.Grid.Columns}.");
            }

            if (spawner.Row < 0 || spawner.Row >= config.Grid.Rows)
            {
                throw new ConfigurationException(
                    $"Spawner row {spawner.Row} is outside the grid height {config.Grid.Rows}.");
            }

            if (spawner.SpawnableItems.Count == 0)
            {
                throw new ConfigurationException(
                    $"Spawner at ({spawner.Column},{spawner.Row}) has no spawnableItems entries.");
            }

            int totalWeight = 0;
            foreach (SpawnableItemConfig si in spawner.SpawnableItems)
            {
                if (!allNames.Contains(si.ItemName))
                {
                    throw new ConfigurationException(
                        $"Spawner at ({spawner.Column},{spawner.Row}) references unknown item '{si.ItemName}'.");
                }

                totalWeight += si.Weight;
            }

            if (totalWeight <= 0)
            {
                throw new ConfigurationException(
                    $"Spawner at ({spawner.Column},{spawner.Row}) has no positive weights.");
            }
        }
    }

    private static void CollectAndValidateChain(ItemChainConfig item, HashSet<string> names)
    {
        if (string.IsNullOrWhiteSpace(item.Name))
        {
            throw new ConfigurationException("Every item must have a non-empty name.");
        }

        if (!names.Add(item.Name))
        {
            throw new ConfigurationException($"Duplicate item name '{item.Name}' found in items configuration.");
        }

        if (item.Product is not null)
        {
            CollectAndValidateChain(item.Product, names);
        }
    }
}
