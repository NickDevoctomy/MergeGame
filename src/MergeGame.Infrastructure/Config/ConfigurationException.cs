namespace MergeGame.Infrastructure.Config;

/// <summary>Thrown when <c>game.json</c> is missing, malformed, or contains invalid values.</summary>
public sealed class ConfigurationException : Exception
{
    /// <summary>Initializes a new instance of the <see cref="ConfigurationException"/> class.</summary>
    public ConfigurationException(string message)
        : base(message)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="ConfigurationException"/> class.</summary>
    public ConfigurationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
