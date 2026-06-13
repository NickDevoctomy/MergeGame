# CLAUDE.md

This file points Claude Code at the shared, cross-tool instructions so there is a single source of
truth.

**Read [`AGENTS.md`](AGENTS.md) and follow every standard in the [`agent-standards/`](agent-standards/)
folder.** All architecture, C# style, async, EF Core, API, testing, and security rules are defined
there.

## Quick reference

- Architecture & boundaries: [`agent-standards/10-architecture.md`](agent-standards/10-architecture.md)
- SOLID design principles: [`agent-standards/15-solid-design.md`](agent-standards/15-solid-design.md)
- C# style: [`agent-standards/20-csharp-style.md`](agent-standards/20-csharp-style.md)
- Security (OWASP): [`agent-standards/70-security-owasp.md`](agent-standards/70-security-owasp.md)

## Commands (do not execute here — build happens on a separate machine)

- Build: `dotnet build`
- Test: `dotnet test`
- Run API: `dotnet run --project src/YourApp.Api`

<!-- YOUR ADDITIONS: keep substantive rules in agent-standards/; this file should stay a thin pointer. -->
