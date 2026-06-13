using MergeGame.Domain;

namespace MergeGame.Application.Results;

/// <summary>The result of activating a spawner.</summary>
public abstract class ActivateSpawnerResult
{
    private ActivateSpawnerResult()
    {
    }

    /// <summary>A new item was successfully placed on the grid.</summary>
    public sealed class Success : ActivateSpawnerResult
    {
        /// <summary>Initializes a new instance of the <see cref="Success"/> class.</summary>
        public Success(MergeItem placedItem)
            : base()
        {
            ArgumentNullException.ThrowIfNull(placedItem);
            PlacedItem = placedItem;
        }

        /// <summary>Gets the item that was placed by the spawner.</summary>
        public MergeItem PlacedItem { get; }
    }

    /// <summary>The grid has no empty cells; the spawner could not place an item.</summary>
    public sealed class GridFull : ActivateSpawnerResult
    {
        /// <summary>Initializes a new instance of the <see cref="GridFull"/> class.</summary>
        public GridFull()
            : base()
        {
        }
    }

    /// <summary>The targeted cell is not a spawner.</summary>
    public sealed class NotASpawner : ActivateSpawnerResult
    {
        /// <summary>Initializes a new instance of the <see cref="NotASpawner"/> class.</summary>
        public NotASpawner()
            : base()
        {
        }
    }
}
