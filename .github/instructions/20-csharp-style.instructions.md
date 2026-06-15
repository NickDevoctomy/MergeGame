---
applyTo: "src/**/*.cs,tests/**/*.cs"
---

# C# Coding Style & Language Standards

You are a senior C# developer. All generated code must follow modern .NET conventions. These rules
are also enforced mechanically by [`.editorconfig`](../.editorconfig) and by **StyleCop** — keep all
three in sync.

## Analyzers — StyleCop is mandatory

- **StyleCop is enabled solution-wide and is non-negotiable.** It is wired into
  [`Directory.Build.props`](../Directory.Build.props) (the `StyleCop.Analyzers` package) and
  configured by [`stylecop.json`](../stylecop.json).
- Because `TreatWarningsAsErrors` is on, **every StyleCop violation fails the build**. Fix violations
  as they arise — never leave them outstanding and never disable the analyzer to make code compile.
- Do **not** blanket-suppress StyleCop rules. If a rule genuinely does not fit the project, change it
  once in [`stylecop.json`](../stylecop.json) so the decision is shared. Suppress an individual rule
  inline only with a written justification on the `#pragma`/`SuppressMessage`.
- Treat StyleCop, the built-in .NET analyzers, and `.editorconfig` as one cohesive style gate.

## Formatting and structure

- **Braces:** Allman style — every brace on its own line. Always use braces, even for
  single-statement `if`/`for`/`while` blocks.
- **Indentation:** 4 spaces, no tabs.
- **Namespaces:** file-scoped (e.g., `namespace YourApp.Domain;`).
- **Usings:** place outside the namespace, sort alphabetically with `System.*` first, and remove
  unused usings.
- **File layout:** one top-level type per file; file name matches the type name.
- DO NOT use region blocks
- DO NOT use comments to separate sections of code
- Code should be self documenting, and comments should only be used where something strange / complex has been implemented that needs clarification.

## Naming

- **PascalCase:** classes, structs, records, interfaces, enums, methods, properties, public fields,
  constants.
- **camelCase:** parameters and local variables.
- **Private fields:** prefix with `_` (e.g., `_userRepository`).
- **Interfaces:** prefix with `I` (e.g., `IUserService`).
- **Async methods:** suffix with `Async`.
- **Type parameters:** prefix with `T` (e.g., `TEntity`).

## Language guidelines

- **Nullable reference types:** `<Nullable>enable</Nullable>` solution-wide (set in
  [`Directory.Build.props`](../Directory.Build.props)). Handle all nullable warnings. Do **not** use
  the null-forgiving operator `!` to silence warnings without justification.
- **`var`:** use only when the type is obvious from the right-hand side (e.g.,
  `var users = new List<User>();`). Use explicit types otherwise.
- **Immutability:** prefer `record` types and `init`-only properties for DTOs and value objects.
- **Expression-bodied members:** acceptable for trivial one-liners; use block bodies otherwise.
- **Pattern matching & switch expressions:** prefer them over long `if/else` chains.
- **`using` declarations:** use for `IDisposable`/`IAsyncDisposable` resources.
- **Modern collection expressions** (`[]`) and primary constructors are allowed where they improve
  clarity.
- **Constants & magic values:** no magic numbers or strings — promote to named constants or config.


## Errors and logging

- Throw the most specific exception type; never throw `System.Exception` directly.
- Do not swallow exceptions silently. Log with structured logging (`ILogger<T>` message templates,
  not string interpolation) and rethrow or handle deliberately.

<!-- PROJECT-SPECIFIC OVERRIDES
e.g., preferred analyzers, file headers/copyright, allowed suppressions, naming exceptions.
-->
