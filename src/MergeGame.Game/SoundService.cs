using System;
using System.Collections.Generic;
using System.IO;

using MergeGame.Application;
using MergeGame.Infrastructure.Config;

using Microsoft.Xna.Framework.Audio;

namespace MergeGame.Game;

/// <summary>Plays sounds loaded from disk using MonoGame <see cref="SoundEffect"/>.</summary>
internal sealed class SoundService : ISoundService, IDisposable
{
    private readonly Dictionary<string, SoundEffect> _sounds = new Dictionary<string, SoundEffect>();
    private bool _disposed;

    internal SoundService(SoundConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);

        if (!config.Enabled)
        {
            return;
        }

        foreach (KeyValuePair<string, string> entry in config.SoundFiles)
        {
            string fullPath = Path.IsPathRooted(entry.Value)
                ? entry.Value
                : Path.Combine(AppContext.BaseDirectory, entry.Value);

            if (File.Exists(fullPath))
            {
                using FileStream stream = File.OpenRead(fullPath);
                _sounds[entry.Key] = SoundEffect.FromStream(stream);
            }
        }
    }

    /// <inheritdoc/>
    public void PlaySound(string key)
    {
        if (_disposed)
        {
            return;
        }

        if (_sounds.TryGetValue(key, out SoundEffect? effect))
        {
            effect.Play();
        }
    }

    /// <summary>Releases all loaded <see cref="SoundEffect"/> instances.</summary>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        foreach (SoundEffect effect in _sounds.Values)
        {
            effect.Dispose();
        }

        _sounds.Clear();
    }
}
