namespace MergeGame.Infrastructure.Config;

/// <summary>Loads <see cref="GameConfig"/> from a JSON source.</summary>
public interface IConfigLoader
{
    /// <summary>Loads and returns the game configuration from <c>game.json</c> in the working directory.</summary>
    /// <returns>The parsed <see cref="GameConfig"/>.</returns>
    public GameConfig Load();
}
