# Plan: MergeGame (MonoGame, net10.0)

## TL;DR

MonoGame merge puzzle game — click a spawner to produce items, drag one item onto a matching item
to merge them into a higher-level item. Domain and Application layers are pure C# with full unit-test
coverage. Infrastructure handles config loading and tile generation. The MonoGame project is the
rendering/input shell.

- **Namespace:** `MergeGame`
- **Target framework:** `net10.0`
- **MonoGame flavour:** `DesktopGL` (cross-platform, no DirectX dependency)
- **Tile art:** generated dynamically at startup (no content pipeline, no pre-made assets)
- **Font:** embedded 5×7 pixel bitmap font scaled to tile size
- **Merge interaction:** drag an item onto a matching item to merge; ghost tile follows cursor
- **Spawner:** click (press + release on same cell) to place a random item in a random empty cell

---

## Phase 0 — Persist This Plan  ✅

Write `memory_bank/PLAN.md` to the workspace before any implementation begins.
Update at the end of each phase to mark it complete and note deviations.

---

## Phase 1 — Solution Scaffolding  ✅

**Deliverable:** `dotnet build` compiles a blank solution with all projects referencing each other.

Projects:
- `src/MergeGame.Domain` — class library, zero external deps
- `src/MergeGame.Application` — references Domain
- `src/MergeGame.Infrastructure` — references Domain + Application; adds `System.Text.Json`
- `src/MergeGame.Game` — MonoGame DesktopGL project; references Application + Infrastructure
- `tests/MergeGame.UnitTests` — xUnit; references Domain + Application + Infrastructure

NuGet:
- `MonoGame.Framework.DesktopGL` → Game only
- `Microsoft.Extensions.DependencyInjection` → Game + Infrastructure
- `xunit`, `xunit.runner.visualstudio`, `FluentAssertions`, `NSubstitute` → UnitTests

Verification: `dotnet build` — zero errors, zero StyleCop warnings.

---

## Phase 2 — Domain Layer  ✅

**Deliverable:** All domain types compile; full unit test suite is green.

| Type | Kind | Notes |
|---|---|---|
| `GridPosition` | `readonly record struct` | `(Column, Row)` |
| `ItemLevel` | `readonly record struct` | wraps `int Value ≥ 1`; exposes `Next()` |
| `MergeItem` | `sealed class` | `Level + Position`, immutable |
| `SpawnerDefinition` | `sealed class` | `IReadOnlyDictionary<ItemLevel, int> Weights`; total weight > 0 |
| `CellContent` | `abstract class` | `Empty / Item(MergeItem) / Spawner(SpawnerDefinition)` |
| `MergeResult` | `abstract class` | `Success(MergeItem) / Failure(string)` |
| `IMergeRule` | interface | `CanMerge` + `Merge` |
| `StandardMergeRule` | `sealed class` | same level → level+1; rejects at max level |
| `IRandomProvider` | interface | `int Next(int min, int max)` |
| `SpawnerService` | `sealed class` | weighted random pick from `SpawnerDefinition` |
| `MergeGrid` | `sealed class` | 10×10 internal array; `PlaceItem`, `TryMerge`, `FindEmptyCells`, `PlaceSpawner` |

Unit test coverage: merge rule happy/edge/max-level; grid boundary protection; spawner weight
distribution (seeded mock); grid-full scenario.

Verification: `dotnet test` — all domain tests green.

---

## Phase 3 — Application Layer  ✅

**Deliverable:** All use-case handlers tested; domain fully orchestrated through commands.

| Type | Notes |
|---|---|
| `IGameSession` | exposes `MergeGrid Grid` + `IMergeRule MergeRule` |
| `GameSession` | implements `IGameSession` |
| `ActivateSpawnerCommand` | record `(GridPosition SpawnerPosition)` |
| `ActivateSpawnerResult` | `Success(MergeItem) / GridFull / NotASpawner` |
| `ActivateSpawnerHandler` | pick random empty cell → `SpawnerService.SpawnItem` → `grid.PlaceItem` |
| `MergeItemsCommand` | record `(GridPosition Source, GridPosition Target)` |
| `MergeItemsResult` | `Success(MergeItem) / Failed(string)` |
| `MergeItemsHandler` | validates positions → `grid.TryMerge` |
| `GetGameStateQuery` | empty record |
| `GetGameStateHandler` | maps `MergeGrid` → `GameStateSnapshot` |
| `GameStateSnapshot` | DTO wrapping `IReadOnlyList<CellStateDto>` |
| `CellStateDto` | `(Position, Level?, IsSpawner, IsEmpty)` |

Verification: `dotnet test` — all application tests green.

---

## Phase 4 — Infrastructure Layer  ✅

**Deliverable:** Config loads from JSON; coloured numbered tiles generated in memory; unit tests
cover config parsing and tile pixel output.

Config POCOs (`GameConfig.cs`):
```
GridConfig    { int Columns, int Rows }
SpawnerConfig { int Column, int Row, Dictionary<int,int> Weights }
TileConfig    { int TileSize, int MaxLevel, Dictionary<int,string> LevelColors }
SoundConfig   { bool Enabled, Dictionary<string,string> SoundFiles }
GameConfig    { GridConfig Grid, List<SpawnerConfig> Spawners,
                TileConfig Tiles, SoundConfig Sounds }
```

Key types:
- `IConfigLoader` → `JsonConfigLoader` (loads `game.json`; exposes `ParseConfig(string)` for tests)
- `ConfigurationException` — thrown on invalid config
- `TileData` — raw RGBA byte array + dimensions
- `ITileGenerator` → `BmpTileGenerator` (solid colour background, auto-HSL for unknown levels,
  embedded 5×7 bitmap font scaled to tile size, white text)
- `SystemRandomProvider` — `IRandomProvider` wrapping `System.Random`
- `game.json` — committed to `src/MergeGame.Game/`; controls all visual/game settings

Verification: `dotnet test` — config and tile tests green; `game.json` loads without error.

---

## Phase 5 — MonoGame Shell  ✅

**Deliverable:** Window opens; coloured 10×10 grid renders; mouse click maps to correct
`GridPosition`.

| Type | Notes |
|---|---|
| `MergeGameMain` | `Game` subclass; `Initialize` builds DI + places spawners; `LoadContent` creates textures |
| `CompositionRoot` | static `Build(GameConfig)` → `ServiceProvider` |
| `GameAssetManager` | wraps `ITileGenerator`; creates + caches `Texture2D` from `TileData` |
| `GridRenderer` | `SpriteBatch`-based; uses `GetGameStateHandler` + `GameAssetManager` |
| `InputHandler` | click → `GridPosition`; spawner click → `ActivateSpawnerCommand`; item click → select/merge |

Click logic:
1. Click any cell: try `ActivateSpawnerCommand` → if `NotASpawner`, treat as item click
2. Item click with nothing selected → select it
3. Item click with same cell selected → deselect
4. Item click with different cell selected → `MergeItemsCommand`; clear selection

DI lifetimes: `IMergeRule`, `IRandomProvider`, `SpawnerService`, `MergeGrid`, `IGameSession`,
`ITileGenerator` → Singleton. Handlers → Transient.

Verification: window opens; coloured grid renders; clicking logs correct `GridPosition`; spawner
activates on click.

---

## Phase 6 — Drag-and-Drop + Playability Polish  ✅

**Other polish:**
1. Drag source border — green border around the cell being dragged from
2. Failed merge flash — red border on target cell for 20 frames
3. Grid-full guard — spawner click no-ops visually when no empty cells
4. Window title shows move count (`Merge Game — Moves: N`)
5. Pixel-perfect rendering — `SamplerState.PointClamp` on `SpriteBatch.Begin`

**Changed types:**
- `MoveItemCommand` (Application) — new record `(GridPosition Source, GridPosition Target)`
- `MoveItemResult` (Application) — `Success / Failed(string)`
- `MoveItemHandler` (Application) — validates source has item, target is empty, moves it
- `MergeGrid.MoveItem` (Domain) — removes from source, places at target
- `InputHandler` — on release, if target is empty issue `MoveItemCommand`, else issue `MergeItemsCommand`

Verification: drag item to empty cell → it moves; drag onto matching item → merge; move count increments.

---

## Phase 8 — Named Item Chains + Image Assets

**Deliverable:** Items have names, descriptions, and PNG images from `src/Resources/`.
Merge chains are configured as a linked tree in JSON — no code changes to add a new theme.

### Problem with the current design

The current system uses a numeric `ItemLevel` (1, 2, 3…) and `levelColors` in config. This
couples the domain to a fixed integer progression and makes it impossible to name items or
assign per-item artwork. It must be replaced.

### New config format (`game.json`)

Top-level `items` array replaces `tiles.levelColors` and the integer-level convention.
Each entry describes one item in the chain; the next tier is its `product`:

```jsonc
{
  "grid": { "columns": 10, "rows": 10 },
  "tileSize": 64,
  "items": [
    {
      "name": "Wood Chips",
      "description": "Small pieces of scrap wood",
      "image": "Resources/1.png",
      "product": {
        "name": "Wood Sticks",
        "description": "Small wooden sticks",
        "image": "Resources/2.png",
        "product": {
          "name": "Plank",
          "description": "A rough wooden plank",
          "image": "Resources/3.png",
          "product": null
        }
      }
    }
  ],
  "spawners": [
    {
      "column": 0, "row": 0,
      "spawnableItems": [
        { "itemName": "Wood Chips", "weight": 60 },
        { "itemName": "Wood Sticks", "weight": 30 }
      ]
    }
  ],
  "sounds": { "enabled": false, "soundFiles": {} }
}
```

> Multiple root items (e.g. a Wood chain AND a Stone chain in the same `items` array)
> are supported. Spawners reference items by name.

### Domain changes

| Type | Change |
|---|---|
| `ItemDefinition` | **New** value object: `string Name`, `string Description`, `string ImagePath`, `ItemDefinition? Product` |
| `ItemLevel` | **Removed** — replaced by `ItemDefinition` identity |
| `MergeItem` | `Level` property becomes `ItemDefinition Definition` |
| `SpawnerDefinition` | Weights keyed by `ItemDefinition` instead of `ItemLevel` |
| `IMergeRule` / `StandardMergeRule` | `CanMerge` checks same `Definition`; `Merge` returns item with `Definition.Product` |
| `MergeGrid` | Internal array stores `MergeItem` keyed by `GridPosition`; no level integer |

### Application changes

| Type | Change |
|---|---|
| `CellStateDto` | `Level?` replaced by `string? ItemName`, `string? ImagePath` |
| `GameStateSnapshot` | No change |
| All handlers | Reference `ItemDefinition` not `ItemLevel` |

### Infrastructure changes

| Type | Change |
|---|---|
| `ItemChainConfig` | **New** POCO: `Name`, `Description`, `Image`, `ItemChainConfig? Product` |
| `SpawnableItemConfig` | **New** POCO: `ItemName`, `Weight` |
| `SpawnerConfig` | `Weights` replaced by `List<SpawnableItemConfig> SpawnableItems` |
| `TileConfig` | `LevelColors` removed; `TileSize` stays |
| `GameConfig` | `Tiles` simplified; `List<ItemChainConfig> Items` added |
| `JsonConfigLoader` | Parses `items` tree; validates names unique, products exist, weights > 0 |
| `IImageLoader` | **New** interface: `Texture2D Load(string path, GraphicsDevice)` |
| `PngImageLoader` | Loads PNGs from disk via `File.OpenRead` + `Texture2D.FromStream` |
| `ITileGenerator` / `BmpTileGenerator` | **Removed** — replaced by `PngImageLoader` |
| `GameAssetManager` | Loads textures from `ItemDefinition.ImagePath`; caches by name |

### Game layer changes

- `CompositionRoot` registers `IImageLoader` → `PngImageLoader`; removes `ITileGenerator`
- `GameAssetManager` loads item textures from PNG files instead of generating them
- `GridRenderer` looks up texture by `cell.ImagePath` (or falls back to a blank tile)
- `ConfigLoader` flattens the `ItemChainConfig` tree into an ordered `List<ItemDefinition>`
  and builds a `Dictionary<string, ItemDefinition>` by name for O(1) lookup

### Unit test updates

- Replace `ItemLevel`-based domain tests with `ItemDefinition`-based equivalents
- Add `JsonConfigLoader` tests for the new `items` tree format
- Add `PngImageLoader` integration test (loads a real PNG from `src/Resources/`)
- Remove `BmpTileGeneratorTests` (generator deleted)

Verification: game loads `game.json`; PNG images render on tiles; merge chain works end-to-end with
named items; spawner uses item names from config.

**Deliverable:** Interfaces + config schema in place; dropping audio files + updating `game.json`
enables sound with zero code changes.

1. `ISoundService` (Application) — `PlaySound(string key)`; `NullSoundService` no-op default
2. `IAnimationService` (Application) — `PlayAnimation(string key, GridPosition pos)`; no-op default
3. `SoundService` (Infrastructure) — loads WAV via MonoGame `SoundEffect`; checks `SoundConfig.Enabled`
4. Call sites: merge success → `"merge"`, spawn → `"spawn"`, invalid merge → `"invalid"`
5. `game.json` `sounds` block pre-populated with expected keys + placeholder paths

Verification: `ISoundService.PlaySound("merge")` called on merge; no exception; config validates.

---

## Key Decisions

| Decision | Choice | Reason |
|---|---|---|
| Namespace | `MergeGame` | Derived from folder name |
| Target framework | `net10.0` | Confirmed by user |
| MonoGame flavour | `DesktopGL` | Cross-platform, no DirectX |
| Tile art | Dynamic generation | No MGCB/pre-made assets required |
| Font | Embedded 5×7 bitmap | Avoids content pipeline; upgradeable to SpriteFont in Phase 6 |
| Spawner weights | Per-spawner `Dictionary<int,int>` in JSON | Zero code changes to add weights |
| Multiple spawners | `List<SpawnerConfig>` in JSON | Adding a spawner = JSON change only |
| Merge interaction | Drag-and-drop (replaced click-select in Phase 6) | More intuitive; ghost tile gives clear visual feedback |
| Move to empty cell | Drag to empty cell = move (Phase 6) | Natural extension of drag UX |
| Item identity | Named `ItemDefinition` chain (replaces integer `ItemLevel`) | Supports arbitrary themes; artwork per item |
| Tile art | PNG files from `src/Resources/` (Phase 7) | User-supplied images; no MGCB needed |
| Spawner weights | Per-spawner `List<SpawnableItemConfig>` by item name | Decoupled from integer levels |
| IRandomProvider | Injected interface | Enables deterministic unit tests |
| Excluded | Multiplayer, save/load, leaderboard | Out of scope |

---

## Relevant Files

- [Directory.Build.props](../Directory.Build.props) — net10.0, StyleCop, Nullable, TreatWarningsAsErrors
- [stylecop.json](../stylecop.json) — StyleCop configuration (xmlHeader: false, documentExposedElements: false)
- [.editorconfig](../.editorconfig) — Allman braces, file-scoped namespaces, _camelCase fields
- [agent-standards/10-architecture.md](../agent-standards/10-architecture.md) — layer boundary rules
- [agent-standards/60-testing.md](../agent-standards/60-testing.md) — xUnit, Given_When_Then, AAA
