# Merge Game

A MonoGame merge puzzle game built on a clean, fully unit-tested core framework. Drag matching
tiles together to merge them into a higher-tier item. Spawn new tiles from configurable spawner
cells. Every rule, every item, and every image is driven by JSON config � no code changes required
to add a new theme.

---

## How to play

- **Click a spawner cell** (green `+` tile) to produce a new item in a random empty cell.
- **Drag an item** to an empty cell to move it.
- **Drag an item** onto another item of the same type to merge them into the next tier.
- Failed merges flash red. The window title shows your current move count.

---

## Building and running

```bash
# Restore & build all projects
dotnet build src/MergeGame.sln

# Run the game
dotnet run --project src/MergeGame.Game

# Run all unit tests
dotnet test src/MergeGame.sln
```

> **Note:** The solution file lives inside `src/`. Run all commands from the repo root
> or `cd src` first.

---

## Configuring the game

Edit `src/MergeGame.Game/game.json`. The config drives everything � no recompilation needed.

### Grid and tile size

```json
{
  "grid": { "columns": 10, "rows": 10 },
  "tileSize": 64
}
```

### Item chains

Items are defined as a nested chain. Two parents produce one child. Add as many tiers as you like.
Each item needs a unique `name`, an optional `description`, and a path to a PNG image asset.

```json
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
]
```

Multiple independent chains (e.g. Wood **and** Stone) are supported � add a second root entry to
the `items` array.

### Spawners

Each spawner references item names from the chain and assigns a relative spawn weight.

```json
"spawners": [
  {
    "column": 0,
    "row": 0,
    "spawnableItems": [
      { "itemName": "Wood Chips", "weight": 60 },
      { "itemName": "Wood Sticks", "weight": 30 },
      { "itemName": "Plank",       "weight": 10 }
    ]
  }
]
```

### Adding your own images

Place PNG files anywhere relative to the game executable (e.g. `Resources/my-item.png`) and
reference the path in `game.json`. The files in `src/Resources/` are copied to the output
directory automatically by the build.

---

## Architecture

The codebase follows **Clean / Onion architecture** with strict layer boundaries:

```
MergeGame.Domain          Zero dependencies � entities, value objects, domain rules
MergeGame.Application     Use cases and handlers � depends on Domain only
MergeGame.Infrastructure  Config loading (JSON) � depends on Domain + Application
MergeGame.Game            MonoGame shell, rendering, input � composition root
tests/MergeGame.UnitTests xUnit tests for Domain, Application, and Infrastructure
```

Key design decisions:

| Decision | Choice |
|---|---|
| Item identity | `ItemDefinition` (name + image + chain link) � no integer levels |
| Merge rule | Two items of the same `ItemDefinition` ? one item of `Definition.Product` |
| Config format | Nested JSON `items` tree; spawners reference items by name |
| Tile artwork | PNG files loaded at runtime via `Texture2D.FromStream`; no content pipeline |
| Interaction | Drag-and-drop; drag to empty cell = move, drag to same type = merge |
| Randomness | `IRandomProvider` interface � injected, deterministically testable |
| DI | `Microsoft.Extensions.DependencyInjection`; singletons for domain, transient for handlers |

---

## Project structure

```
src/
  MergeGame.Domain/           Entities, rules, interfaces
  MergeGame.Application/      Commands, results, handlers, DTOs
  MergeGame.Infrastructure/   Config loader, random provider
  MergeGame.Game/             MonoGame entry point, rendering, input
    game.json                 All game configuration
  Resources/                  PNG tile images (1.png � 10.png)
tests/
  MergeGame.UnitTests/        46 unit tests
memory_bank/                  Project context and plan (PLAN.md)
```
