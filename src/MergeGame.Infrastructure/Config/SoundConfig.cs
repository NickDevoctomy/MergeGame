using System.Collections.Generic;

namespace MergeGame.Infrastructure.Config;

/// <summary>Sound configuration loaded from configuration. Reserved for Phase 7.</summary>
public sealed class SoundConfig
{
    /// <summary>Gets or sets a value indicating whether sound is enabled.</summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets the mapping of sound keys to file paths.
    /// Example keys: <c>"merge"</c>, <c>"spawn"</c>, <c>"invalid"</c>.
    /// </summary>
    public Dictionary<string, string> SoundFiles { get; set; } = [];
}
