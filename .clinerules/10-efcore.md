---
paths:
  - "**/Infrastructure/**"
  - "**/Persistence/**"
  - "**/Repositories/**"
  - "**/*DbContext*"
---

# EF Core Data Access (Cline)

Follow the canonical standard: `agent-standards/40-efcore-data-access.md`.

Summary: `.AsNoTracking()` on reads, explicit `.Include()` (no lazy loading), project into DTOs,
restrict `SaveChangesAsync()` to the use-case boundary, optimistic concurrency, pass
`CancellationToken`, no hard-coded connection strings.
