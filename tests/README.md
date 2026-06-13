# `tests/` — Test projects

**All test projects live here**, separate from production code in [`../src/`](../src).

## Recommended layout

```
tests/
└── YourApp.UnitTests       xUnit unit tests
```

Add integration or functional test projects alongside as needed (e.g.,
`YourApp.IntegrationTests`). Replace `YourApp` with your project's namespace prefix.

## Rules

- Follow the testing standards in [`../agent-standards/60-testing.md`](../agent-standards/60-testing.md)
  (xUnit, AAA, `Method_Should_When` naming, deterministic and isolated tests).
