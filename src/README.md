# `src/` — Application source code

**All production source code lives here.** Create one project per Clean Architecture layer in this
directory; nothing application-related belongs at the repository root.

## Recommended layout

```
src/
├── YourApp.Domain          entities, value objects, interfaces (no outward dependencies)
├── YourApp.Application      use cases, DTOs (depends on Domain only)
├── YourApp.Infrastructure   EF Core, repositories, external services
└── YourApp.Api              ASP.NET Core composition root
```

Replace `YourApp` with your project's namespace prefix.

## Rules

- Keep tests out of here — they go in the sibling [`../tests/`](../tests) directory.
- Respect the layer boundaries defined in
  [`../agent-standards/10-architecture.md`](../agent-standards/10-architecture.md).
- Style and analysis (including mandatory StyleCop) are enforced by
  [`../Directory.Build.props`](../Directory.Build.props) and [`../.editorconfig`](../.editorconfig).
