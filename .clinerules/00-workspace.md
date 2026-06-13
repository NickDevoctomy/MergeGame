# Workspace Conventions (Cline)

You are the primary engineering agent for this C#/.NET workspace. The full standards live in the
[`agent-standards/`](agent-standards/) folder — this is a thin pointer so there is a single source of
truth. Read the file relevant to what you are editing.

## First action (new workspace)

Before writing or scaffolding any code, confirm the project's namespace prefix (the `YourApp`
placeholder) — ask one question unless it's already given. Apply it as you create files. See
`agent-standards/05-onboarding.md`.

## Always

- C# style & naming → `agent-standards/20-csharp-style.md`
- Async & concurrency → `agent-standards/30-async.md`
- Security (OWASP) → `agent-standards/70-security-owasp.md`
- Architecture & boundaries → `agent-standards/10-architecture.md`
- SOLID design principles → `agent-standards/15-solid-design.md`
- Self-correction / anti-loop → `agent-standards/90-loop-circuit-breaker.md`

## Summary

- Clean/Onion architecture, namespace prefix `YourApp` (placeholder), `net9.0`.
- Allman braces, file-scoped namespaces, `#nullable enable`, `Async` suffix; never import EF Core or
  ASP.NET into the Domain layer; inject dependencies.
- Prioritise reading files in `memory_bank/` to rebuild context, and keep it updated.

> Do not build/publish here — the maintainer builds on a separate machine.
