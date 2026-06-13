# Project Brief

## Project

Merge Game — a MonoGame puzzle game where the player merges identical items on a grid to produce
higher-level items, using a spawner to introduce new pieces.

## Namespace

`MergeGame`

## Goals

- Playable 10×10 merge grid driven entirely by `game.json` — no recompile needed for visual/config changes.
- Core logic thoroughly unit-tested (Domain + Application layers have no MonoGame dependency).
- Spawner cell: click to place a random item in a random empty cell (weighted by config).
- Tiles dynamically generated at startup from config colours + embedded bitmap font.
- Phase roadmap through sound/animation stubs: see `memory_bank/PLAN.md`.

## Success Criteria

- `dotnet build` — zero errors, zero StyleCop warnings.
- `dotnet test` — all tests green.
- Game window opens; clicking spawner produces items; matching items merge with incrementing number.

## Core requirement

- **Scope:** _TODO — one or two sentences describing what this project delivers._

## Goals

- _TODO_

## Success criteria

- Code compiles with zero warnings under nullable reference rules.
- _TODO — add measurable criteria._

## Out of scope

- _TODO_
