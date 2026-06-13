# GitHub Copilot — Repository Instructions

These are repository-wide instructions for GitHub Copilot. The full, canonical standards live in the
[`agent-standards/`](../agent-standards/) folder — this file is a thin pointer so everything stays in
one place. Path-scoped rules live in [`.github/instructions/`](instructions/).

## Always follow the shared standards

> **First, in a new/empty workspace:** before writing or scaffolding any code, confirm the project's
> namespace prefix (the `YourApp` placeholder) by asking the user one question — unless it's already
> stated in the prompt, folder name, or an existing project. Then apply it as you create files. See
> [`agent-standards/05-onboarding.md`](../agent-standards/05-onboarding.md).

Read and apply the standard relevant to the file you are editing:

- Onboarding (do this first) → [`agent-standards/05-onboarding.md`](../agent-standards/05-onboarding.md)
- Architecture & layer boundaries → [`agent-standards/10-architecture.md`](../agent-standards/10-architecture.md)
- SOLID design principles → [`agent-standards/15-solid-design.md`](../agent-standards/15-solid-design.md)
- C# style & naming → [`agent-standards/20-csharp-style.md`](../agent-standards/20-csharp-style.md)
- Async & concurrency → [`agent-standards/30-async.md`](../agent-standards/30-async.md)
- EF Core data access → [`agent-standards/40-efcore-data-access.md`](../agent-standards/40-efcore-data-access.md)
- ASP.NET Core API → [`agent-standards/50-api-aspnet.md`](../agent-standards/50-api-aspnet.md)
- Testing → [`agent-standards/60-testing.md`](../agent-standards/60-testing.md)
- Security (OWASP) → [`agent-standards/70-security-owasp.md`](../agent-standards/70-security-owasp.md)
- Self-correction → [`agent-standards/90-loop-circuit-breaker.md`](../agent-standards/90-loop-circuit-breaker.md)

## Summary

- Modern .NET, Clean/Onion architecture, namespace prefix `YourApp` (placeholder), target `net9.0`.
- Allman braces, file-scoped namespaces, `#nullable enable`, `Async` suffix on async methods.
- Never import EF Core or ASP.NET into the Domain layer; inject dependencies.
- Flow `CancellationToken` through async chains; no hard-coded secrets; validate all input.
- If a request conflicts with a standard, stop and explain rather than violating it.

> Do not build or publish in this workspace — the maintainer builds on a separate machine.

<!-- YOUR ADDITIONS: prefer adding a file under agent-standards/custom/ and linking it above. -->
