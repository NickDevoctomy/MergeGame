# Agent Standards — Overview (Single Source of Truth)

> **This folder is the one place to edit your engineering standards.**
> Every AI assistant (GitHub Copilot, Cursor, Cline, Roo Code, Windsurf, Claude, Gemini/Codex)
> is wired to read these files. Platform-specific files under `.github/`, `.cursor/`, `.clinerules/`,
> `.roo/`, and `.windsurf/` are **thin adapters that reference these documents by path** — they
> contain no rules of their own. Change a rule here once and it applies everywhere.

## How this repository is organized

| Path | Purpose | Who edits it |
| --- | --- | --- |
| `agent-standards/*.md` | Canonical coding standards (the real rules) | **You** |
| `agent-standards/custom/` | Your own additional standards files | **You** |
| `AGENTS.md` (root) | Cross-tool entry point read natively by most agents | Generated once; rarely edit |
| `.github/`, `.cursor/`, `.clinerules/`, `.roo/`, `.windsurf/` | Platform adapters → point back here | Rarely edit |
| `memory_bank/` | Persistent project context across agent sessions | **You** + the agent |
| `src/`, `tests/` | Your application source (`src/`) and test projects (`tests/`) | **You** |

## Standards index

0. [`05-onboarding.instructions.md`](05-onboarding.instructions.md) — **Do this first** in a new workspace: confirm the project name.
1. [`10-architecture.instructions.md`](10-architecture.instructions.md) — Clean/Onion layer boundaries and allowed dependencies.
2. [`15-solid-design.instructions.md`](15-solid-design.instructions.md) — SOLID object-oriented design principles.
3. [`20-csharp-style.instructions.md`](20-csharp-style.instructions.md) — Language conventions, formatting, naming.
4. [`30-async.instructions.md`](30-async.instructions.md) — Asynchronous and concurrency rules.
5. [`40-efcore-data-access.instructions.md`](40-efcore-data-access.instructions.md) — Entity Framework Core performance and integrity.
6. [`50-api-aspnet.instructions.md`](50-api-aspnet.instructions.md) — ASP.NET Core API conventions.
7. [`60-testing.instructions.md`](60-testing.instructions.md) — Testing standards and structure.
8. [`70-security-owasp.instructions.md`](70-security-owasp.instructions.md) — Security baseline (OWASP Top 10).
9. [`90-loop-circuit-breaker.instructions.md`](90-loop-circuit-breaker.instructions.md) — Anti-loop / self-correction protocol.

## Operating principles for every agent

- **Read the standard for the layer you are editing before generating code.** Layers are defined in
  [`10-architecture.instructions.md`](10-architecture.instructions.md).
- **Prefer reading the real files on disk** over relying on cached assumptions.
- **If a request would violate a standard, stop, explain the conflict, and propose a compliant
  alternative** rather than silently breaking the rule.
- **Ask exactly one targeted question** when a required decision is genuinely ambiguous; otherwise
  proceed with the most idiomatic .NET approach.
- **Keep `memory_bank/` current** when architectural decisions are made (see
  [`../memory_bank/LEARNING_LOG.md`](../memory_bank/LEARNING_LOG.md)).

## Project facts to fill in

These values are referenced throughout the standards. On a fresh drop into a new workspace, the agent
confirms them during onboarding (see [`05-onboarding.instructions.md`](05-onboarding.instructions.md)) rather than expecting you
to edit them by hand.

- **Top-level namespace prefix:** `YourApp` _(placeholder — the agent will ask for and apply the real
  name; see [`05-onboarding.instructions.md`](05-onboarding.instructions.md))_
- **Target framework:** `net10.0`
- **Primary database engine:** _TODO (e.g., PostgreSQL, SQL Server)_
- **Test framework:** `xUnit`

<!-- PROJECT-SPECIFIC OVERRIDES
Add cross-cutting notes that don't fit a single topic file here, or create a new file under
agent-standards/custom/ and link it from this index.
-->
