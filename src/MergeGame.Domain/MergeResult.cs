namespace MergeGame.Domain;

/// <summary>The result of a merge attempt.</summary>
public abstract class MergeResult
{
    private MergeResult()
    {
    }

    /// <summary>Merge succeeded and produced a new item.</summary>
    public sealed class Success : MergeResult
    {
        /// <summary>Initializes a new instance of the <see cref="Success"/> class.</summary>
        public Success(MergeItem producedItem)
            : base()
        {
            ArgumentNullException.ThrowIfNull(producedItem);
            ProducedItem = producedItem;
        }

        /// <summary>Gets the item produced by the merge.</summary>
        public MergeItem ProducedItem { get; }
    }

    /// <summary>Merge failed for the given reason.</summary>
    public sealed class Failure : MergeResult
    {
        /// <summary>Initializes a new instance of the <see cref="Failure"/> class.</summary>
        public Failure(string reason)
            : base()
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(reason);
            Reason = reason;
        }

        /// <summary>Gets a human-readable explanation of why the merge failed.</summary>
        public string Reason { get; }
    }
}
