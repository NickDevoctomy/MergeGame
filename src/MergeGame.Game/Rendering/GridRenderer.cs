using MergeGame.Application.Dtos;
using MergeGame.Application.Handlers;
using MergeGame.Application.Queries;
using MergeGame.Domain;
using MergeGame.Infrastructure.Config;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MergeGame.Game.Rendering;

/// <summary>Draws the merge grid using a <see cref="SpriteBatch"/>.</summary>
internal sealed class GridRenderer
{
    private const int BorderThickness = 3;

    private readonly SpriteBatch _spriteBatch;
    private readonly GameAssetManager _assetManager;
    private readonly GetGameStateHandler _getGameStateHandler;
    private readonly int _tileSize;
    private readonly int _offsetX;
    private readonly int _offsetY;

    internal GridRenderer(
        SpriteBatch spriteBatch,
        GameAssetManager assetManager,
        GetGameStateHandler getGameStateHandler,
        GameConfig config,
        int padding)
    {
        ArgumentNullException.ThrowIfNull(spriteBatch);
        ArgumentNullException.ThrowIfNull(assetManager);
        ArgumentNullException.ThrowIfNull(getGameStateHandler);
        ArgumentNullException.ThrowIfNull(config);

        _spriteBatch = spriteBatch;
        _assetManager = assetManager;
        _getGameStateHandler = getGameStateHandler;
        _tileSize = config.Tiles.TileSize;
        _offsetX = padding;
        _offsetY = padding;
    }

    internal void Draw(
        GridPosition? dragSource,
        bool isDragging,
        Point cursorPos,
        GridPosition? failedTarget)
    {
        GameStateSnapshot snapshot = _getGameStateHandler.Handle(new GetGameStateQuery());

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        foreach (CellStateDto cell in snapshot.Cells)
        {
            DrawCell(cell, dragSource, failedTarget);
        }

        if (isDragging && dragSource.HasValue)
        {
            DrawDragGhost(snapshot, dragSource.Value, cursorPos);
        }

        _spriteBatch.End();
    }

    private void DrawCell(CellStateDto cell, GridPosition? dragSource, GridPosition? failedTarget)
    {
        Texture2D texture = ResolveTexture(cell);
        int x = _offsetX + (cell.Position.Column * _tileSize);
        int y = _offsetY + (cell.Position.Row * _tileSize);

        bool isBeingDragged = dragSource.HasValue && dragSource.Value == cell.Position;
        Color tint = isBeingDragged ? Color.White * 0.4f : Color.White;

        _spriteBatch.Draw(texture, new Rectangle(x, y, _tileSize, _tileSize), tint);

        if (isBeingDragged)
        {
            DrawBorder(x, y, Color.LimeGreen);
        }

        if (failedTarget.HasValue && failedTarget.Value == cell.Position)
        {
            DrawBorder(x, y, Color.Red);
        }
    }

    private void DrawDragGhost(GameStateSnapshot snapshot, GridPosition source, Point cursorPos)
    {
        foreach (CellStateDto cell in snapshot.Cells)
        {
            if (cell.Position != source || cell.IsEmpty || cell.IsSpawner)
            {
                continue;
            }

            Texture2D ghostTex = _assetManager.GetTile(cell.Level!.Value);
            int halfTile = _tileSize / 2;

            _spriteBatch.Draw(
                ghostTex,
                new Rectangle(cursorPos.X - halfTile, cursorPos.Y - halfTile, _tileSize, _tileSize),
                Color.White * 0.75f);

            break;
        }
    }

    private Texture2D ResolveTexture(CellStateDto cell)
    {
        if (cell.IsSpawner)
        {
            return _assetManager.SpawnerTile;
        }

        if (cell.IsEmpty)
        {
            return _assetManager.EmptyTile;
        }

        return _assetManager.GetTile(cell.Level!.Value);
    }

    private void DrawBorder(int x, int y, Color color)
    {
        DrawHorizontalLine(x, y, _tileSize, BorderThickness, color);
        DrawHorizontalLine(x, y + _tileSize - BorderThickness, _tileSize, BorderThickness, color);
        DrawVerticalLine(x, y, _tileSize, BorderThickness, color);
        DrawVerticalLine(x + _tileSize - BorderThickness, y, _tileSize, BorderThickness, color);
    }

    private void DrawHorizontalLine(int x, int y, int width, int thickness, Color color)
    {
        _spriteBatch.Draw(
            _assetManager.EmptyTile,
            new Rectangle(x, y, width, thickness),
            color);
    }

    private void DrawVerticalLine(int x, int y, int height, int thickness, Color color)
    {
        _spriteBatch.Draw(
            _assetManager.EmptyTile,
            new Rectangle(x, y, thickness, height),
            color);
    }
}
