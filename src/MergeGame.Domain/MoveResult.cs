namespace MergeGame.Domain;

/// <summary>The result of a move-item operation on the grid.</summary>
public abstract class MoveResult
{
    private MoveResult()
    {
    }

    /// <summary>The move succeeded.</summary>
    public sealed class Success : MoveResult
    {
        /// <summary>Initializes a new instance of the <see cref="Success"/> class.</summary>
        public Success(MergeItem movedItem)
            : base()
        {
            ArgumentNullException.ThrowIfNull(movedItem);
            MovedItem = movedItem;
        }

        /// <summary>Gets the item that was moved to its new position.</summary>
        public MergeItem MovedItem { get; }
    }

    /// <summary>The move failed for the given reason.</summary>
    public sealed class Failure : MoveResult
    {
        /// <summary>Initializes a new instance of the <see cref="Failure"/> class.</summary>
        public Failure(string reason)
            : base()
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(reason);
            Reason = reason;
        }

        /// <summary>Gets a human-readable explanation of the failure.</summary>
        public string Reason { get; }
    }
}
