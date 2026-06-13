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
- Follow **Arrange / Act / Assert**, separated by blank lines.
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

## Quality rules

- Tests must be **deterministic and isolated** — no shared mutable state, no ordering dependence, no
  reliance on wall-clock time (inject `TimeProvider`) or real network calls.
- Prefer one logical assertion per test; group tightly-related assertions.
- Use builders/object mothers for test data, not sprawling inline setup.
- Mock only abstractions you own; do not mock types you don't control.
- Keep tests fast; mark slow integration tests with a trait/category so they can be filtered.

## Commands (run on your build machine)

> Per project convention, the agent does **not** build or run the solution here. These are for
> reference:
- Run all tests: `dotnet test`
- Run a single test: `dotnet test --filter "FullyQualifiedName~MyTestName"`

<!-- PROJECT-SPECIFIC OVERRIDES
e.g., chosen assertion/mocking libraries, coverage threshold, snapshot testing, test categories.
-->
