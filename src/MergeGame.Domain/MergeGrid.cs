using System.Collections.Generic;

namespace MergeGame.Domain;

/// <summary>The aggregate root representing the merge grid state.</summary>
public sealed class MergeGrid
{
    private readonly CellContent[,] _cells;

    /// <summary>Initializes a new instance of the <see cref="MergeGrid"/> class.</summary>
    public MergeGrid(int columns, int rows)
    {
        if (columns <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(columns), "Column count must be greater than zero.");
        }

        if (rows <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rows), "Row count must be greater than zero.");
        }

        Columns = columns;
        Rows = rows;
        _cells = new CellContent[columns, rows];

        for (int c = 0; c < columns; c++)
        {
            for (int r = 0; r < rows; r++)
            {
                _cells[c, r] = CellContent.Empty.Instance;
            }
        }
    }

    /// <summary>Gets the number of columns in the grid.</summary>
    public int Columns { get; }

    /// <summary>Gets the number of rows in the grid.</summary>
    public int Rows { get; }

    /// <summary>Returns the content of the cell at <paramref name="position"/>.</summary>
    public CellContent GetCell(GridPosition position)
    {
        ValidatePosition(position);
        return _cells[position.Column, position.Row];
    }

    /// <summary>Places <paramref name="item"/> at its embedded position. Throws if the cell is occupied.</summary>
    public void PlaceItem(MergeItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        ValidatePosition(item.Position);

        if (_cells[item.Position.Column, item.Position.Row] is not CellContent.Empty)
        {
            throw new InvalidOperationException(
                $"Cell ({item.Position.Column},{item.Position.Row}) is already occupied.");
        }

        _cells[item.Position.Column, item.Position.Row] = new CellContent.Item(item);
    }

    /// <summary>Places a spawner at <paramref name="position"/>. Throws if the cell is occupied.</summary>
    public void PlaceSpawner(GridPosition position, SpawnerDefinition definition)
    {
        ArgumentNullException.ThrowIfNull(definition);
        ValidatePosition(position);

        if (_cells[position.Column, position.Row] is not CellContent.Empty)
        {
            throw new InvalidOperationException(
                $"Cell ({position.Column},{position.Row}) is already occupied.");
        }

        _cells[position.Column, position.Row] = new CellContent.Spawner(definition);
    }

    /// <summary>
    /// Removes the spawner at <paramref name="position"/>, replacing it with an empty cell.
    /// Throws if the cell does not contain a spawner.
    /// </summary>
    public void RemoveSpawner(GridPosition position)
    {
        ValidatePosition(position);

        if (_cells[position.Column, position.Row] is not CellContent.Spawner)
        {
            throw new InvalidOperationException(
                $"Cell ({position.Column},{position.Row}) does not contain a spawner.");
        }

        _cells[position.Column, position.Row] = CellContent.Empty.Instance;
    }

    /// <summary>
    /// Attempts to merge the item at <paramref name="sourcePosition"/> into the item at
    /// <paramref name="targetPosition"/> using <paramref name="rule"/>.
    /// </summary>
    public MergeResult TryMerge(GridPosition sourcePosition, GridPosition targetPosition, IMergeRule rule)
    {
        ArgumentNullException.ThrowIfNull(rule);
        ValidatePosition(sourcePosition);
        ValidatePosition(targetPosition);

        if (_cells[sourcePosition.Column, sourcePosition.Row] is not CellContent.Item sourceCell)
        {
            return new MergeResult.Failure("Source cell does not contain an item.");
        }

        if (_cells[targetPosition.Column, targetPosition.Row] is not CellContent.Item targetCell)
        {
            return new MergeResult.Failure("Target cell does not contain an item.");
        }

        if (!rule.CanMerge(sourceCell.MergeItem, targetCell.MergeItem))
        {
            return new MergeResult.Failure(
                $"Cannot merge '{sourceCell.MergeItem.Definition.Name}' with '{targetCell.MergeItem.Definition.Name}'.");
        }

        MergeItem produced = rule.Merge(sourceCell.MergeItem, targetCell.MergeItem, targetPosition);
        _cells[sourcePosition.Column, sourcePosition.Row] = CellContent.Empty.Instance;
        _cells[targetPosition.Column, targetPosition.Row] = new CellContent.Item(produced);

        return new MergeResult.Success(produced);
    }

    /// <summary>Returns all grid positions whose cells are empty.</summary>
    public IReadOnlyList<GridPosition> FindEmptyCells()
    {
        var result = new List<GridPosition>();

        for (int c = 0; c < Columns; c++)
        {
            for (int r = 0; r < Rows; r++)
            {
                if (_cells[c, r] is CellContent.Empty)
                {
                    result.Add(new GridPosition(c, r));
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Moves the item at <paramref name="sourcePosition"/> to <paramref name="targetPosition"/>.
    /// Returns <see cref="MoveResult.Failure"/> if the source has no item or the target is not empty.
    /// </summary>
    public MoveResult MoveItem(GridPosition sourcePosition, GridPosition targetPosition)
    {
        ValidatePosition(sourcePosition);
        ValidatePosition(targetPosition);

        if (_cells[sourcePosition.Column, sourcePosition.Row] is not CellContent.Item sourceCell)
        {
            return new MoveResult.Failure("Source cell does not contain an item.");
        }

        if (_cells[targetPosition.Column, targetPosition.Row] is not CellContent.Empty)
        {
            return new MoveResult.Failure("Target cell is not empty.");
        }

        MergeItem moved = sourceCell.MergeItem.WithPosition(targetPosition);
        _cells[sourcePosition.Column, sourcePosition.Row] = CellContent.Empty.Instance;
        _cells[targetPosition.Column, targetPosition.Row] = new CellContent.Item(moved);

        return new MoveResult.Success(moved);
    }

    private void ValidatePosition(GridPosition position)
    {
        if (position.Column < 0 || position.Column >= Columns
            || position.Row < 0 || position.Row >= Rows)
        {
            throw new ArgumentOutOfRangeException(
                nameof(position),
                $"Position ({position.Column},{position.Row}) is outside the {Columns}×{Rows} grid.");
        }
    }
}
