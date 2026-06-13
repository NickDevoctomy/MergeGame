# Agentic C# Boilerplate

A ready-to-use workspace template for building **C# / .NET applications with AI coding agents**. It
gives every major agent platform the same high-quality engineering standards from a **single source
of truth**, so your code stays consistent no matter which assistant you (or your team) use.

> This repo contains **instructions and configuration only** — no application source code. Add your
> own `src/` and `tests/` projects when you start building.

---

## How it works: one source of truth, many adapters

All the actual rules live **once** in [`agent-standards/`](agent-standards/). Every platform-specific
file is a thin **adapter** that points back to those canonical documents by relative path. Update a
standard in one place and every agent picks up the change — no duplication, no drift.

```
agent-standards/          ← the canonical rules (edit these)
   ├── 00-overview.md          index + project facts
   ├── 10-architecture.md      Clean/Onion layer boundaries
   ├── 15-solid-design.md      SOLID design principles
   ├── 20-csharp-style.md      formatting & naming
   ├── 30-async.md             async/await & cancellation
   ├── 40-efcore-data-access.md EF Core patterns
   ├── 50-api-aspnet.md        ASP.NET Core API design
   ├── 60-testing.md           xUnit testing standards
   ├── 70-security-owasp.md    OWASP Top 10
   ├── 90-loop-circuit-breaker.md self-correction protocol
   └── custom/                 ← YOUR standards go here
```

### Which file does each platform read?

| Platform | Entry point(s) | Mechanism |
| --- | --- | --- |
| **Cross-tool** (Cursor, Cline, Roo, Copilot coding agent, Codex, Gemini) | [`AGENTS.md`](AGENTS.md) | Read natively from repo root |
| **Claude Code** | [`CLAUDE.md`](CLAUDE.md) | Thin pointer to `AGENTS.md` |
| **Gemini** | [`GEMINI.md`](GEMINI.md) | Thin pointer to `AGENTS.md` |
| **GitHub Copilot** | [`.github/copilot-instructions.md`](.github/copilot-instructions.md) + [`.github/instructions/`](.github/instructions/) | Repo-wide file + `applyTo` globbed instruction files |
| **Cursor** | [`.cursor/rules/`](.cursor/rules/) | `*.mdc` rules with `description`/`globs`/`alwaysApply` |
| **Cline** | [`.clinerules/`](.clinerules/) | Folder of `.md` rules, optional `paths:` scoping |
| **Roo Code** | [`.roo/rules/`](.roo/rules/) | Folder read recursively; also reads `AGENTS.md` |
| **Windsurf** | [`.windsurf/rules/`](.windsurf/rules/) + [`.windsurf/workflows/`](.windsurf/workflows/) | Rules with `trigger:` modes |

Every one of those adapter files simply links back to the matching document in `agent-standards/`.

---

## Where **you** add your own things

This template is laid out so your additions stay clearly separated from the shared backbone:

1. **Your own coding standards →** drop a markdown file in
   [`agent-standards/custom/`](agent-standards/custom/) and link it from
   [`agent-standards/00-overview.md`](agent-standards/00-overview.md). All platforms inherit it
   automatically because they reference the `agent-standards/` folder.
2. **Repo-wide tweaks →** each platform's entry file has a clearly marked
   `<!-- YOUR ADDITIONS -->` block at the bottom.
3. **Project context for agents →** fill in the templates in [`memory_bank/`](memory_bank/).
4. **Tooling config →** [`.editorconfig`](.editorconfig),
   [`Directory.Build.props`](Directory.Build.props), and [`stylecop.json`](stylecop.json) enforce the
   C# style, mandatory StyleCop analysis, and compiler settings in the build itself.

---

## Getting started

1. **Just start prompting — the agent asks for the project name.** On a fresh drop into a new
   workspace, the agent's first action is to confirm your namespace prefix (the `YourApp`
   placeholder) and apply it as it creates files. No manual find/replace needed. See
   [`agent-standards/05-onboarding.md`](agent-standards/05-onboarding.md). _(You can still set the
   name yourself up front if you prefer.)_
2. **Adjust the target framework** in [`Directory.Build.props`](Directory.Build.props) if you aren't
   on `net9.0`.
3. **Add your projects.** The recommended layout (see [`AGENTS.md`](AGENTS.md)):
   ```
   src/YourApp.Domain          entities, value objects, interfaces (no dependencies)
   src/YourApp.Application      use cases, DTOs (depends on Domain)
   src/YourApp.Infrastructure   EF Core, repositories, external services
   src/YourApp.Api              ASP.NET Core composition root
   tests/YourApp.UnitTests      xUnit tests
   ```
4. **Add your own standards** under [`agent-standards/custom/`](agent-standards/custom/).
5. **Keep `memory_bank/` current** as the project evolves.

---

## Project conventions at a glance

- Clean / Onion architecture with strict layer boundaries.
- Allman braces, file-scoped namespaces, `#nullable enable`, `Async` suffix on async methods.
- Flow `CancellationToken` through async chains.
- No hard-coded secrets; validate all external input; deny-by-default authorization.

See [`agent-standards/00-overview.md`](agent-standards/00-overview.md) for the full index.

> **Note:** Build and publish happen on a separate machine — agents are instructed **not** to run
> `dotnet build`, `dotnet test`, or `dotnet publish` in this workspace.
