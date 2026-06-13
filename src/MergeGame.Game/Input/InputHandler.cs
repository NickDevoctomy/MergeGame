using MergeGame.Application.Commands;
using MergeGame.Application.Handlers;
using MergeGame.Application.Results;
using MergeGame.Domain;
using MergeGame.Infrastructure.Config;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MergeGame.Game.Input;

/// <summary>Translates mouse input into game commands using drag-and-drop interaction.</summary>
internal sealed class InputHandler
{
    private const int FlashDuration = 20;

    private readonly ActivateSpawnerHandler _activateSpawnerHandler;
    private readonly MergeItemsHandler _mergeItemsHandler;
    private readonly MoveItemHandler _moveItemHandler;
    private readonly int _tileSize;
    private readonly int _offsetX;
    private readonly int _offsetY;
    private readonly int _columns;
    private readonly int _rows;

    private MouseState _previousMouseState;
    private GridPosition? _dragSource;
    private bool _isDragging;
    private GridPosition? _failedMergeTarget;
    private int _flashFrames;

    internal InputHandler(
        ActivateSpawnerHandler activateSpawnerHandler,
        MergeItemsHandler mergeItemsHandler,
        MoveItemHandler moveItemHandler,
        GameConfig config,
        int padding)
    {
        ArgumentNullException.ThrowIfNull(activateSpawnerHandler);
        ArgumentNullException.ThrowIfNull(mergeItemsHandler);
        ArgumentNullException.ThrowIfNull(moveItemHandler);
        ArgumentNullException.ThrowIfNull(config);

        _activateSpawnerHandler = activateSpawnerHandler;
        _mergeItemsHandler = mergeItemsHandler;
        _moveItemHandler = moveItemHandler;
        _tileSize = config.TileSize;
        _offsetX = padding;
        _offsetY = padding;
        _columns = config.Grid.Columns;
        _rows = config.Grid.Rows;
    }

    /// <summary>Gets the current screen-space mouse position.</summary>
    internal Point MouseScreenPosition { get; private set; }

    /// <summary>Gets the grid cell where the current drag started, or <see langword="null"/> when the mouse button is up.</summary>
    internal GridPosition? DragSource => _dragSource;

    /// <summary>Gets a value indicating whether the cursor has moved to a different cell since the drag began.</summary>
    internal bool IsDragging => _isDragging;

    /// <summary>Gets the target cell that should flash red after a failed merge, or <see langword="null"/> when no flash is active.</summary>
    internal GridPosition? FailedMergeTarget => _flashFrames > 0 ? _failedMergeTarget : null;

    /// <summary>Gets the number of successful merges performed this session.</summary>
    internal int MoveCount { get; private set; }

    internal void Update()
    {
        MouseState current = Mouse.GetState();
        MouseScreenPosition = new Point(current.X, current.Y);

        bool buttonJustPressed = current.LeftButton == ButtonState.Pressed
                                 && _previousMouseState.LeftButton == ButtonState.Released;
        bool buttonJustReleased = current.LeftButton == ButtonState.Released
                                  && _previousMouseState.LeftButton == ButtonState.Pressed;

        if (buttonJustPressed)
        {
            _dragSource = ScreenToGrid(current.X, current.Y);
            _isDragging = false;
        }

        if (current.LeftButton == ButtonState.Pressed && _dragSource.HasValue)
        {
            GridPosition? hovered = ScreenToGrid(current.X, current.Y);

            if (hovered.HasValue && hovered.Value != _dragSource.Value)
            {
                _isDragging = true;
            }
        }

        if (buttonJustReleased && _dragSource.HasValue)
        {
            GridPosition source = _dragSource.Value;
            GridPosition? target = ScreenToGrid(current.X, current.Y);

            HandleRelease(source, target);

            _dragSource = null;
            _isDragging = false;
        }

        if (_flashFrames > 0)
        {
            _flashFrames--;
        }

        _previousMouseState = current;
    }

    private void HandleRelease(GridPosition source, GridPosition? target)
    {
        bool isDrop = target.HasValue && target.Value != source;

        if (!isDrop)
        {
            _activateSpawnerHandler.Handle(new ActivateSpawnerCommand(source));
            return;
        }

        // Try move first — succeeds only when the target cell is empty.
        MoveItemResult moveResult = _moveItemHandler.Handle(new MoveItemCommand(source, target!.Value));

        if (moveResult is MoveItemResult.Success)
        {
            MoveCount++;
            return;
        }

        // Target is occupied — attempt a merge.
        MergeItemsResult mergeResult = _mergeItemsHandler.Handle(new MergeItemsCommand(source, target!.Value));

        if (mergeResult is MergeItemsResult.Success)
        {
            MoveCount++;
        }
        else
        {
            _failedMergeTarget = target;
            _flashFrames = FlashDuration;
        }
    }

    private GridPosition? ScreenToGrid(int x, int y)
    {
        int col = (x - _offsetX) / _tileSize;
        int row = (y - _offsetY) / _tileSize;

        if (col >= 0 && col < _columns && row >= 0 && row < _rows)
        {
            return new GridPosition(col, row);
        }

        return null;
    }
}
