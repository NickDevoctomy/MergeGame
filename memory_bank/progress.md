# Progress

## What works

- Solution scaffolded: 5 projects + MergeGame.sln
- Phase 1 complete: all .csproj files, Directory.Build.props inherits net10.0 / StyleCop
- Phase 2 complete: all Domain types (GridPosition, ItemLevel, MergeItem, SpawnerDefinition,
  CellContent, MergeResult, IMergeRule, StandardMergeRule, IRandomProvider, SpawnerService, MergeGrid)
- Phase 3 complete: Application layer (IGameSession, GameSession, all commands, results, handlers,
  DTOs, queries)
- Phase 4 complete: Infrastructure layer (GameConfig POCO hierarchy, JsonConfigLoader with
  ParseConfig, ConfigurationException, TileData, ITileGenerator, BmpTileGenerator with embedded
  5×7 pixel font, SystemRandomProvider); game.json committed
- Phase 5 complete: MonoGame shell (MergeGameMain, CompositionRoot, GameAssetManager,
  GridRenderer, InputHandler, Program.cs)
- All unit tests written: Domain (StandardMergeRule, MergeGrid, SpawnerService),
  Application (ActivateSpawnerHandler, MergeItemsHandler, GetGameStateHandler),
  Infrastructure (JsonConfigLoader, BmpTileGenerator)

## What's left to build

- Phase 5 verification: build and run the game window on device
- Phase 6: selection highlight, invalid merge flash, grid-full guard, move counter, max level cap
- Phase 7: ISoundService / IAnimationService stubs wired at call sites

## Known issues

- _TODO._
