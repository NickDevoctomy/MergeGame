# System Patterns

## High-level architecture

- Clean / Onion architecture. See [`../agent-standards/10-architecture.md`](../agent-standards/10-architecture.md)
  for the layer boundaries the agents enforce.
- _TODO — diagram or summary of the major components._

## Key implementation decisions

- **Dependency injection lifetimes:** Scoped for `DbContext` and repositories; Transient for
  stateless domain services; Singleton for configuration/engines.
- **Data transfer:** strict separation between persistence models and API contracts (DTOs).
- _TODO — add project-specific decisions._
