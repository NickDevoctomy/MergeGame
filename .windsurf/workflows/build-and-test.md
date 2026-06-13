---
description: "Build and test the .NET solution (run on the maintainer's build machine)."
---

# Workflow: Build and Test

> Note: this repository is not built in this workspace — the maintainer builds on a separate machine.
> This workflow documents the canonical sequence for reference.

## Steps

1. **Restore & build**
   - `dotnet build --configuration Release`
   - If compile errors occur, apply the circuit-breaker protocol in
     `agent-standards/90-loop-circuit-breaker.md`.
2. **Run tests**
   - `dotnet test --no-build --configuration Release`
   - If tests fail, analyze the failing run and fix per `agent-standards/60-testing.md`.
