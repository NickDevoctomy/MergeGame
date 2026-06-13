# Testing Standards

You are a test engineer. Apply these rules to all test code (`tests/**`).

## Framework & structure

- Test framework: **xUnit**. Assertions: prefer a fluent assertion library for readability; mocking:
  a standard mocking library for interfaces.
- One test class per subject under test; mirror the source namespace/folder structure.
  - The instance variable of the subject under test should be named 'sut'
- Name tests `Given_When_Then`
  - e.g. GivenCondition_AndOtherCondition_AndAntherCondition_WhenMethod_ThenOutcome_AndOtherOutcome_AndAnotherOutcome
  - Be descriptive
- Each section of the test should be prefaced by a single line comment of 'Arrange', 'Act' or 'Assert'.
- Do not add any additional comments
- Try to keep Act line to a single 'sut' call where possible

## What to test

- **Domain & Application logic:** thorough unit tests — these are the highest-value tests and need no
  database.
- **Infrastructure:** integration tests against a real or containerized database (e.g., Testcontainers)
  rather than mocking the `DbContext`.
- **API:** endpoint/integration tests via `WebApplicationFactory`.
- Cover the happy path, edge cases, and failure/exception paths. Add a regression test for every bug
  fixed.
- Cover every reachable branch when writing new production code.

## Quality rules

- Tests must be **deterministic and isolated** — no shared mutable state, no ordering dependence, no
  reliance on wall-clock time (inject `TimeProvider`) or real network calls.
- Prefer one logical assertion per test; group tightly-related assertions.
- Use builders/object mothers for test data, not sprawling inline setup.
- Mock only abstractions you own; do not mock types you don't control.
- Keep tests fast; mark slow integration tests with a trait/category so they can be filtered.
- Do not write tests that only assert a trivially true condition — each test must exercise real logic.

## Commands (run on your build machine)

> Per project convention, the agent does **not** build or run the solution here. These are for
> reference:
- Run all tests: `dotnet test`
- Run a single test: `dotnet test --filter "FullyQualifiedName~MyTestName"`
- Run with coverage: `dotnet test --collect:"XPlat Code Coverage" --results-directory TestResults`

## Code coverage

- Use **Coverlet** (`coverlet.collector`) for coverage collection. Add it to the test project if not already present: `dotnet add <TestProject>.csproj package coverlet.collector`.
- Coverage is collected in **Cobertura XML** format by default.

### What counts

- **Must be covered:** all domain logic, happy paths, guard clauses (`ArgumentNullException`,
  `ArgumentException`), failure branches, and every new bug fix.
- **Thin wrappers with 0% coverage:** write at least one smoke test confirming the class constructs
  and its public method does not throw.
- **Excluded from threshold obligations:** infrastructure I/O wrappers (file/network reads),
  DI composition root, and platform shell code — these cannot be meaningfully unit-tested in
  isolation and should be verified end-to-end.
- After adding tests, rerun coverage and confirm no tracked class has dropped below its threshold.

<!-- PROJECT-SPECIFIC OVERRIDES
e.g., chosen assertion/mocking libraries, coverage threshold, snapshot testing, test categories.
-->
