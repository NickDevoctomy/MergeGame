# SOLID Design Principles

You are an expert object-oriented designer. Apply the five SOLID principles when designing types,
especially in the Domain and Application layers (see [`10-architecture.instructions.md`](10-architecture.instructions.md)).
These principles produce code that is easy to extend, test, and reason about. Favor them — but do
not over-engineer: introduce an abstraction when there is a real, present need, not speculatively.

## S — Single Responsibility Principle (SRP)

- A class should have **one reason to change**. Keep each type focused on a single concern.
- Split types that mix concerns (e.g., a handler that validates, maps, queries, and formats output).
- Use-case handlers orchestrate; they delegate persistence to repositories, mapping to mappers, and
  validation to validators.
- **Smell:** a class name containing "And", "Manager", "Helper", or "Util" that keeps growing.

## O — Open/Closed Principle (OCP)

- Types should be **open for extension, closed for modification**. Add behavior without editing
  existing, tested code.
- Prefer polymorphism, strategy injection, and new implementations of an interface over `switch`
  statements that must be edited for every new case.
- **Smell:** repeatedly modifying the same `switch`/`if-else` chain whenever a new variant appears.

## L — Liskov Substitution Principle (LSP)

- Subtypes must be **substitutable for their base type** without breaking callers.
- Implementations must honor the contract of the interface they implement — no strengthening
  preconditions, weakening postconditions, or throwing `NotSupportedException` for inherited members.
- Prefer composition over inheritance when an "is-a" relationship does not truly hold.
- **Smell:** an override that throws, no-ops, or checks the concrete type with `is`/`as`.

## I — Interface Segregation Principle (ISP)

- Prefer **small, focused interfaces** over large, general-purpose ones. Clients should not depend on
  members they do not use.
- Split a "fat" interface into role-based abstractions (e.g., `IOrderReader` and `IOrderWriter`
  instead of one `IOrderRepository` when consumers need only one side).
- **Smell:** implementations forced to provide empty or throwing members.

## D — Dependency Inversion Principle (DIP)

- High-level modules depend on **abstractions**, not concrete details. Both depend on abstractions.
- Define interfaces (ports) in the Domain/Application layers; implement them in Infrastructure. This
  is the mechanism behind the inward-pointing dependencies in [`10-architecture.instructions.md`](10-architecture.instructions.md).
- Inject dependencies through constructors; never `new` up infrastructure inside business logic.
- **Smell:** an Application/Domain type instantiating a `DbContext`, `HttpClient`, or other concrete
  infrastructure directly.

## Applying SOLID without over-engineering

- Introduce an abstraction when there are **two or more real implementations**, a genuine testing
  seam, or a layer boundary to respect — not "just in case".
- A single concrete class with no boundary to cross does not need an interface.
- Keep abstractions cohesive: an interface that exists only to mirror one class one-to-one adds
  indirection without value.
- When a request would force a SOLID violation, stop and propose a compliant design (see the
  operating principles in [`00-overview.instructions.md`](00-overview.instructions.md)).
