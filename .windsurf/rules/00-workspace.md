---
trigger: always_on
description: "Core C#/.NET standards: architecture boundaries, style, async, security."
---

# Workspace Standards (Windsurf)

Follow the canonical standards in the [`agent-standards/`](../../agent-standards/) folder — the single
source of truth. Read the file relevant to what you are editing:

- Onboarding (do this first) → `agent-standards/05-onboarding.md`
- Architecture & boundaries → `agent-standards/10-architecture.md`
- SOLID design principles → `agent-standards/15-solid-design.md`
- C# style & naming → `agent-standards/20-csharp-style.md`
- Async & concurrency → `agent-standards/30-async.md`
- Security (OWASP) → `agent-standards/70-security-owasp.md`
- Self-correction / anti-loop → `agent-standards/90-loop-circuit-breaker.md`

Summary: Clean/Onion architecture (`YourApp` placeholder, `net9.0`); Allman braces, file-scoped namespaces,
`#nullable enable`, `Async` suffix; never import EF Core/ASP.NET into Domain; inject dependencies;
flow `CancellationToken`; no hard-coded secrets.

> Do not build/publish here — the maintainer builds on a separate machine.
