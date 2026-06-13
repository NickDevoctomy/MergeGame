using MergeGame.Application;
using MergeGame.Application.Handlers;
using MergeGame.Game.Input;
using MergeGame.Game.Rendering;
using MergeGame.Infrastructure.Config;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MergeGame.Game;

/// <summary>The MonoGame entry point for Merge Game.</summary>
public sealed class MergeGameMain : Microsoft.Xna.Framework.Game
{
    private const int Padding = 40;

    private readonly GraphicsDeviceManager _graphics;
    private readonly GameConfig _gameConfig;

    private ServiceProvider? _serviceProvider;
    private SpriteBatch? _spriteBatch;
    private GameAssetManager? _assetManager;
    private GridRenderer? _renderer;
    private InputHandler? _inputHandler;

    /// <summary>Initializes a new instance of the <see cref="MergeGameMain"/> class.</summary>
    public MergeGameMain()
    {
        _graphics = new GraphicsDeviceManager(this);

        var configLoader = new JsonConfigLoader();
        _gameConfig = configLoader.Load();

        _graphics.PreferredBackBufferWidth = (_gameConfig.Grid.Columns * _gameConfig.TileSize) + (Padding * 2);
        _graphics.PreferredBackBufferHeight = (_gameConfig.Grid.Rows * _gameConfig.TileSize) + (Padding * 2);
        Window.Title = "Merge Game";
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    /// <inheritdoc/>
    protected override void Initialize()
    {
        _serviceProvider = CompositionRoot.Build(_gameConfig);

        IGameSession session = _serviceProvider.GetRequiredService<IGameSession>();
        CompositionRoot.PlaceSpawners(_gameConfig, session);

        base.Initialize();
    }

    /// <inheritdoc/>
    protected override void LoadContent()
    {
        base.LoadContent();

        _spriteBatch = new SpriteBatch(GraphicsDevice);

        ServiceProvider sp = _serviceProvider!;
        _assetManager = new GameAssetManager(GraphicsDevice, _gameConfig.TileSize);

        ActivateSpawnerHandler activateHandler = sp.GetRequiredService<ActivateSpawnerHandler>();
        MergeItemsHandler mergeHandler = sp.GetRequiredService<MergeItemsHandler>();
        MoveItemHandler moveHandler = sp.GetRequiredService<MoveItemHandler>();
        GetGameStateHandler stateHandler = sp.GetRequiredService<GetGameStateHandler>();

        _inputHandler = new InputHandler(activateHandler, mergeHandler, moveHandler, _gameConfig, Padding);
        _renderer = new GridRenderer(_spriteBatch, _assetManager, stateHandler, _gameConfig, Padding);
    }

    /// <inheritdoc/>
    protected override void Update(GameTime gameTime)
    {
        _inputHandler!.Update();
        Window.Title = $"Merge Game — Moves: {_inputHandler.MoveCount}";
        base.Update(gameTime);
    }

    /// <inheritdoc/>
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.DarkSlateGray);
        _renderer!.Draw(
            _inputHandler!.DragSource,
            _inputHandler.IsDragging,
            _inputHandler.MouseScreenPosition,
            _inputHandler.FailedMergeTarget);
        base.Draw(gameTime);
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _serviceProvider?.Dispose();
        }

        base.Dispose(disposing);
    }
}
