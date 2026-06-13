---
applyTo: "src/**/*.cs,tests/**/*.cs"
---

# C# Source — Scoped Instructions

When editing C# files, follow the canonical standards (do not duplicate them here):

- Coding style & naming → [`agent-standards/20-csharp-style.md`](../../agent-standards/20-csharp-style.md)
- Architecture & layer boundaries → [`agent-standards/10-architecture.md`](../../agent-standards/10-architecture.md)
- Async & concurrency → [`agent-standards/30-async.md`](../../agent-standards/30-async.md)
- Security (OWASP) → [`agent-standards/70-security-owasp.md`](../../agent-standards/70-security-owasp.md)

Key reminders: Allman braces, file-scoped namespaces, `#nullable enable`, `I`-prefixed interfaces,
`_camelCase` private fields, `Async` suffix on async methods, explicit types unless obvious.
