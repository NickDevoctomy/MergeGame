namespace MergeGame.Domain;

/// <summary>Represents the content of a single cell on the merge grid.</summary>
public abstract class CellContent
{
    private CellContent()
    {
    }

    /// <summary>An empty cell containing nothing.</summary>
    public sealed class Empty : CellContent
    {
        private Empty()
            : base()
        {
        }

        /// <summary>Gets the singleton instance of <see cref="Empty"/>.</summary>
        public static Empty Instance { get; } = new Empty();
    }

    /// <summary>A cell occupied by a merge item.</summary>
    public sealed class Item : CellContent
    {
        /// <summary>Initializes a new instance of the <see cref="Item"/> class.</summary>
        public Item(MergeItem mergeItem)
            : base()
        {
            ArgumentNullException.ThrowIfNull(mergeItem);
            MergeItem = mergeItem;
        }

        /// <summary>Gets the merge item occupying this cell.</summary>
        public MergeItem MergeItem { get; }
    }

    /// <summary>A cell occupied by a spawner.</summary>
    public sealed class Spawner : CellContent
    {
        /// <summary>Initializes a new instance of the <see cref="Spawner"/> class.</summary>
        public Spawner(SpawnerDefinition definition)
            : base()
        {
            ArgumentNullException.ThrowIfNull(definition);
            Definition = definition;
        }

        /// <summary>Gets the spawner definition for this cell.</summary>
        public SpawnerDefinition Definition { get; }
    }
}
