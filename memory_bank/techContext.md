# Tech Context

## Runtime

- Target framework: `net10.0`
- Language: C# (LangVersion: latest)
- Nullable reference types: enabled solution-wide

## Game Framework

- MonoGame.Framework.DesktopGL 3.8.2.1105
- MonoGame.Content.Builder.Task 3.8.2.1105

## DI / Config

- Microsoft.Extensions.DependencyInjection 10.0.0
- System.Text.Json (in-box with .NET 10)
- Config loaded from `game.json` at runtime

## Testing

- xUnit 2.9.3
- FluentAssertions 8.2.0
- NSubstitute 5.3.0

## Code Quality

- StyleCop.Analyzers 1.2.0-beta.556 (solution-wide, TreatWarningsAsErrors = true)
- SA1309 suppressed (allow _camelCase fields), SA1101 suppressed (no this. prefix required)
- CA1034 suppressed (public nested types for discriminated unions), CA1707 suppressed in tests

## Architecture

Clean/Onion adapted for a game context:
- `MergeGame.Domain` — entities, value objects, interfaces; zero dependencies
- `MergeGame.Application` — use-case handlers, DTOs; depends on Domain only
- `MergeGame.Infrastructure` — config loading, tile generation; depends on Domain + Application
- `MergeGame.Game` — MonoGame shell, DI composition root; depends on Application + Infrastructure
- `MergeGame.UnitTests` — xUnit tests for Domain + Application + Infrastructure

## Core platform

- **SDK / runtime:** .NET (target `net10.0` — adjust as needed).
- **Database engine:** _TODO (e.g., PostgreSQL, SQL Server)._
- **Entity Framework Core:** _TODO — version._

## Constraints

- Nullable reference types must be `enable` on all projects.
- No raw connection strings or secrets in source — load via the Options pattern / configuration
  providers (see [`../agent-standards/70-security-owasp.md`](../agent-standards/70-security-owasp.md)).
- _TODO — add other technical constraints._

## Key dependencies

- _TODO — list important NuGet packages and why they're used._
