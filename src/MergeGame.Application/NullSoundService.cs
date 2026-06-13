namespace MergeGame.Application;

/// <summary>No-op implementation of <see cref="ISoundService"/> used when sound is disabled.</summary>
public sealed class NullSoundService : ISoundService
{
    /// <inheritdoc/>
    public void PlaySound(string key)
    {
        // Intentionally empty — sound disabled.
    }
}
