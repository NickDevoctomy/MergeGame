---
trigger: glob
globs: "**/Infrastructure/**/*.cs,**/Persistence/**/*.cs,**/Repositories/**/*.cs,**/*DbContext*.cs"
description: "EF Core data access standards."
---

# EF Core Data Access (Windsurf)

Follow the canonical standard: `agent-standards/40-efcore-data-access.md`.

Summary: `.AsNoTracking()` on reads, explicit `.Include()` (no lazy loading), project into DTOs,
restrict `SaveChangesAsync()` to the use-case boundary, optimistic concurrency, pass
`CancellationToken`, no hard-coded connection strings.
