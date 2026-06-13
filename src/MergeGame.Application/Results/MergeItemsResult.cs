using MergeGame.Domain;

namespace MergeGame.Application.Results;

/// <summary>The result of a merge-items command.</summary>
public abstract class MergeItemsResult
{
    private MergeItemsResult()
    {
    }

    /// <summary>The merge succeeded and produced a new item.</summary>
    public sealed class Success : MergeItemsResult
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

    /// <summary>The merge failed for the given reason.</summary>
    public sealed class Failed : MergeItemsResult
    {
        /// <summary>Initializes a new instance of the <see cref="Failed"/> class.</summary>
        public Failed(string reason)
            : base()
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(reason);
            Reason = reason;
        }

        /// <summary>Gets a human-readable explanation of the failure.</summary>
        public string Reason { get; }
    }
}
