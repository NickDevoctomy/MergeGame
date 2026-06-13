---
trigger: glob
globs: "**/Api/**/*.cs,**/Web/**/*.cs"
description: "ASP.NET Core API conventions."
---

# API / Presentation (Windsurf)

Follow the canonical standards: `agent-standards/50-api-aspnet.md` and
`agent-standards/70-security-owasp.md`.

Summary: thin endpoints that delegate to Application use cases, request DTOs (not entities),
ProblemDetails errors, correct status codes, flow `CancellationToken`, authorize by default.
