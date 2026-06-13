namespace MergeGame.Application;

/// <summary>Plays game sounds identified by a string key.</summary>
public interface ISoundService
{
    /// <summary>Plays the sound associated with the given key. Does nothing if the key is unknown.</summary>
    /// <param name="key">The sound key, e.g. <c>"merge"</c>, <c>"spawn"</c>, <c>"invalid"</c>.</param>
    public void PlaySound(string key);
}
