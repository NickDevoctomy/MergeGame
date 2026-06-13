---
applyTo: "**/Infrastructure/**/*.cs,**/Persistence/**/*.cs,**/Repositories/**/*.cs,**/*DbContext*.cs"
---

# Data Access — Scoped Instructions

When editing persistence/EF Core code, follow the canonical standard:

- EF Core data access → [`agent-standards/40-efcore-data-access.md`](../../agent-standards/40-efcore-data-access.md)

Key reminders: `.AsNoTracking()` on reads, explicit `.Include()` (no lazy loading), project into DTOs,
restrict `SaveChangesAsync()` to the use-case boundary, pass `CancellationToken`, no hard-coded
connection strings.
