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

        if (config.Tiles.TileSize <= 0)
        {
            throw new ConfigurationException("tiles.tileSize must be greater than zero.");
        }

        if (config.Tiles.MaxLevel <= 0)
        {
            throw new ConfigurationException("tiles.maxLevel must be greater than zero.");
        }

        foreach (SpawnerConfig spawner in config.Spawners)
        {
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

            int totalWeight = 0;
            foreach (int w in spawner.Weights.Values)
            {
                totalWeight += w;
            }

            if (totalWeight <= 0)
            {
                throw new ConfigurationException(
                    $"Spawner at ({spawner.Column},{spawner.Row}) has no positive weights.");
            }
        }

        foreach (System.Collections.Generic.KeyValuePair<int, string> kvp in config.Tiles.LevelColors)
        {
            ValidateHexColor(kvp.Value, kvp.Key);
        }
    }

    private static void ValidateHexColor(string hex, int level)
    {
        string cleaned = hex.TrimStart('#');

        if (cleaned.Length != 6)
        {
            throw new ConfigurationException(
                $"Level {level} colour '{hex}' is not a valid #RRGGBB hex colour.");
        }

        foreach (char c in cleaned)
        {
            if (!Uri.IsHexDigit(c))
            {
                throw new ConfigurationException(
                    $"Level {level} colour '{hex}' contains non-hex character '{c}'.");
            }
        }
    }
}
