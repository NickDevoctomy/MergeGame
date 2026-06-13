# AGENTS.md — Agent Instructions for this C#/.NET Workspace

> This is the cross-tool entry point. Cursor, Cline, Roo Code, GitHub Copilot (coding agent),
> OpenAI Codex, Gemini, and most modern coding agents read this file natively. The detailed,
> canonical standards live in the [`agent-standards/`](agent-standards/) folder — this file points
> to them so there is a **single source of truth**.

## Read these before writing code

> **First action in a new workspace:** before generating or scaffolding any code, confirm the
> project's namespace prefix (the `YourApp` placeholder) — ask the user one question unless it's
> already given (prompt, folder name, or existing project). Apply it as you create files. See
> [`agent-standards/05-onboarding.md`](agent-standards/05-onboarding.md).

Follow every standard in the `agent-standards/` folder. Read the one relevant to the file you are
editing:

- Architecture & layer boundaries → [`agent-standards/10-architecture.md`](agent-standards/10-architecture.md)
- SOLID design principles → [`agent-standards/15-solid-design.md`](agent-standards/15-solid-design.md)
- C# style & naming → [`agent-standards/20-csharp-style.md`](agent-standards/20-csharp-style.md)
- Async & concurrency → [`agent-standards/30-async.md`](agent-standards/30-async.md)
- EF Core data access → [`agent-standards/40-efcore-data-access.md`](agent-standards/40-efcore-data-access.md)
- ASP.NET Core API → [`agent-standards/50-api-aspnet.md`](agent-standards/50-api-aspnet.md)
- Testing → [`agent-standards/60-testing.md`](agent-standards/60-testing.md)
- Security (OWASP) → [`agent-standards/70-security-owasp.md`](agent-standards/70-security-owasp.md)
- Anti-loop / self-correction → [`agent-standards/90-loop-circuit-breaker.md`](agent-standards/90-loop-circuit-breaker.md)
- Your own additional standards → [`agent-standards/custom/`](agent-standards/custom/)

## Project overview

- A modern .NET solution following **Clean / Onion architecture**.
- Namespace prefix: `YourApp` (placeholder — rename to your project's namespace). Target framework: `net9.0`.
- Recommended layout to adopt when you add projects (replace `YourApp` with your namespace):
  - `src/YourApp.Domain` — entities, value objects, interfaces (no dependencies).
  - `src/YourApp.Application` — use cases, DTOs (depends on Domain).
  - `src/YourApp.Infrastructure` — EF Core, repositories, external services.
  - `src/YourApp.Api` — ASP.NET Core API (composition root).
  - `tests/YourApp.UnitTests` — xUnit tests.

## Build, test & run commands (for reference)

> **Do not run build/publish in this workspace** — the maintainer builds on a separate machine.
> These commands are documented so you can describe steps, not execute them:
- Restore & build: `dotnet build`
- Run tests: `dotnet test`
- Run the API: `dotnet run --project src/YourApp.Api`

## House rules (summary — full detail in `agent-standards/`)

- Allman braces, file-scoped namespaces, `#nullable enable`, `Async` suffix on async methods.
- Respect layer boundaries: never import EF Core or ASP.NET into the Domain layer.
- Inject dependencies; never `new` up infrastructure inside business logic.
- Flow `CancellationToken` through async call chains.
- No hard-coded secrets; validate all external input; deny-by-default authorization.
- If a request conflicts with a standard, stop and explain instead of violating it.

## Keep context current

Update [`memory_bank/`](memory_bank/) when architectural decisions are made, and record non-obvious
decisions/corrections in [`memory_bank/LEARNING_LOG.md`](memory_bank/LEARNING_LOG.md).

<!-- YOUR ADDITIONS
Add project-specific, repo-wide guidance here, or (preferred) add a topic file under
agent-standards/custom/ and link it from the list above.
-->
