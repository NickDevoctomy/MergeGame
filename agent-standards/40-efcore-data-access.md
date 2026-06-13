# Entity Framework Core — Data Access Standards

You are a database engineer specializing in EF Core performance. These rules apply to persistence
code (`**/Infrastructure/**`, `**/Persistence/**`, repositories, and `DbContext` types). They must
**never** appear in the Domain layer — see [`10-architecture.md`](10-architecture.md).

## Query integrity & performance

- **No tracking on reads:** append `.AsNoTracking()` to all read-only queries (queries, list
  endpoints, reporting) to avoid change-tracker overhead.
- **Explicit eager loading:** load related data deliberately with `.Include()` / `.ThenInclude()`.
  Never rely on lazy loading — it causes N+1 query storms.
- **Project early:** prefer `.Select(...)` into DTOs so only required columns are fetched. Avoid
  pulling full entities when a projection suffices.
- **No client-side evaluation of filters:** keep `Where`/`OrderBy` translatable to SQL. Do not filter
  in memory after materializing.
- **Pagination:** never return unbounded result sets. Use `.Skip()/.Take()` (keyset pagination for
  large tables).

## Transactions & saving

- **Restrict `SaveChangesAsync()`** to the command/use-case boundary (Unit of Work). Do **not** call
  it inside individual repositories, loops, or helpers.
- Wrap multi-step writes in an explicit transaction when atomicity is required.
- Pass `CancellationToken` to every async EF call (`ToListAsync`, `FirstOrDefaultAsync`,
  `SaveChangesAsync`, ...).

## Modeling & configuration

- Configure entities with `IEntityTypeConfiguration<T>` classes, not inline in `OnModelCreating`.
- Implement **optimistic concurrency** with a `rowversion`/concurrency token on mutable entities.
- Keep migrations reviewed and named meaningfully; never edit applied migrations — add a new one.
- Store connection strings via the Options pattern / configuration providers — **never** hard-code
  credentials (see [`70-security-owasp.md`](70-security-owasp.md)).

<!-- PROJECT-SPECIFIC OVERRIDES
e.g., chosen provider, naming conventions for tables, soft-delete strategy, audit columns.
-->
