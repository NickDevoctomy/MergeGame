# GitHub Copilot — Repository Instructions

These are repository-wide instructions for GitHub Copilot. The full, canonical standards live in
[`.github/instructions/`](instructions/) alongside the path-scoped rules.

## Always follow the shared standards

> **First, in a new/empty workspace:** before writing or scaffolding any code, confirm the project's
> namespace prefix (the `YourApp` placeholder) by asking the user one question — unless it's already
> stated in the prompt, folder name, or an existing project. Then apply it as you create files. See
> [`./instructions/05-onboarding.instructions.md`](./instructions/05-onboarding.instructions.md).

Read and apply the standard relevant to the file you are editing:

- Onboarding (do this first) → [`./instructions/05-onboarding.instructions.md`](./instructions/05-onboarding.instructions.md)
- Architecture & layer boundaries → [`./instructions/10-architecture.instructions.md`](./instructions/10-architecture.instructions.md)
- SOLID design principles → [`./instructions/15-solid-design.instructions.md`](./instructions/15-solid-design.instructions.md)
- C# style & naming → [`./instructions/20-csharp-style.instructions.md`](./instructions/20-csharp-style.instructions.md)
- Async & concurrency → [`./instructions/30-async.instructions.md`](./instructions/30-async.instructions.md)
- EF Core data access → [`./instructions/40-efcore-data-access.instructions.md`](./instructions/40-efcore-data-access.instructions.md)
- ASP.NET Core API → [`./instructions/50-api-aspnet.instructions.md`](./instructions/50-api-aspnet.instructions.md)
- Testing → [`./instructions/60-testing.instructions.md`](./instructions/60-testing.instructions.md)
- Security (OWASP) → [`./instructions/70-security-owasp.instructions.md`](./instructions/70-security-owasp.instructions.md)
- Self-correction → [`./instructions/90-loop-circuit-breaker.instructions.md`](./instructions/90-loop-circuit-breaker.instructions.md)

## Summary

- Modern .NET, Clean/Onion architecture, namespace prefix `YourApp` (placeholder), target `net10.0`.
- Allman braces, file-scoped namespaces, `#nullable enable`, `Async` suffix on async methods.
- Never import EF Core or ASP.NET into the Domain layer; inject dependencies.
- Flow `CancellationToken` through async chains; no hard-coded secrets; validate all input.
- If a request conflicts with a standard, stop and explain rather than violating it.

## Change Checklist

- Did you follow the shared coding standards?
- Did you build the entire solution to check for compile errors?
- Did you check code coverage?
- Did you check for StyleCop warnings?
- Did you check for any other warnings / messages related to your changes?
- Did you leave in any commented out code or introduce any unecessary comments?
