using MergeGame.Domain;

namespace MergeGame.Application.Results;

/// <summary>The result of a move-item command.</summary>
public abstract class MoveItemResult
{
    private MoveItemResult()
    {
    }

    /// <summary>The item was moved successfully.</summary>
    public sealed class Success : MoveItemResult
    {
        /// <summary>Initializes a new instance of the <see cref="Success"/> class.</summary>
        public Success(MergeItem movedItem)
            : base()
        {
            ArgumentNullException.ThrowIfNull(movedItem);
            MovedItem = movedItem;
        }

        /// <summary>Gets the item at its new position.</summary>
        public MergeItem MovedItem { get; }
    }

    /// <summary>The move failed for the given reason.</summary>
    public sealed class Failed : MoveItemResult
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
