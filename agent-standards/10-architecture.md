# Architectural Guardrails (Clean / Onion Architecture)

You are an expert .NET solution architect. Enforce the layer boundaries below. Do not propose code
that violates them.

## Architectural model

- Style: **Clean / Onion architecture** — dependencies point inward; the Domain has no outward
  dependencies.
- Top-level namespace prefix: `YourApp` _(placeholder — rename to your project's namespace)_

## Solution layout

- **All production source code lives under the `src/` directory**, one project per layer
  (`src/YourApp.Domain`, `src/YourApp.Application`, `src/YourApp.Infrastructure`, `src/YourApp.Api`).
- **All test projects live under the `tests/` directory** (e.g., `tests/YourApp.UnitTests`).
- Do not place `.cs` source files, projects, or the solution's application code at the repository
  root — keep the root for configuration and agent standards only.

## Layer definitions and allowed dependencies

### 1. Domain layer — `**/Domain/**/*.cs`
- **May reference:** nothing. No external framework dependencies whatsoever.
- **Prohibited:** `Microsoft.EntityFrameworkCore`, `Microsoft.AspNetCore.*`, `HttpClient`,
  `IConfiguration`, `ILogger<T>`, and ambient calls like `DateTime.Now` / `DateTime.UtcNow`
  (inject an `IClock`/`TimeProvider` abstraction instead).
- **Contains:** entities, value objects, domain events, domain exceptions, and the **interfaces**
  (ports) the outer layers implement.

### 2. Application layer — `**/Application/**/*.cs`
- **May reference:** Domain only.
- **Prohibited:** direct SQL, `DbContext`, or any concrete infrastructure type. Reach external
  concerns through Domain/Application abstractions.
- **Contains:** use cases (commands/queries/handlers), DTOs, validation, and the interfaces it
  expects Infrastructure to implement.

### 3. Infrastructure / Persistence layer — `**/Infrastructure/**/*.cs`, `**/Persistence/**/*.cs`
- **May reference:** Domain and Application.
- **Contains:** `DbContext` implementations, repositories, file system and external API clients,
  message brokers, and the concrete implementations of Application/Domain interfaces.

### 4. API / Presentation layer — `**/Api/**/*.cs`, `**/Web/**/*.cs`
- **May reference:** Application, and Infrastructure **only for dependency-injection wiring** at
  startup.
- **Contains:** endpoints/controllers, middleware, request/response contracts, and composition root
  (DI registration).

## Cross-cutting rules

- **Dependency injection:** depend on abstractions, not concretions. Never `new` up a database
  context, HTTP client, or gateway inside business logic — inject it.
- **DI lifetimes:** `DbContext` and repositories are **Scoped**; stateless domain services are
  **Transient**; configuration/singletons are **Singleton**. If a lifetime is unclear, ask one
  targeted question.
- **DTO boundary:** never expose Domain entities directly from the API. Map to dedicated
  request/response models.

## Verification protocol

1. Before generating code, identify which layer the target file belongs to.
2. If the change would cross a boundary (e.g., importing EF Core into Domain), **stop**, explain the
   violation, and suggest a compliant pattern.

<!-- PROJECT-SPECIFIC OVERRIDES
e.g., extra layers (BackgroundJobs, Contracts), module boundaries, or namespace conventions.
-->
