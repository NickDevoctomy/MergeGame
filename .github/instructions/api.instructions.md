---
applyTo: "**/Api/**/*.cs,**/Web/**/*.cs"
---

# API / Presentation — Scoped Instructions

When editing API/presentation code, follow the canonical standards:

- ASP.NET Core API conventions → [`agent-standards/50-api-aspnet.md`](../../agent-standards/50-api-aspnet.md)
- Security (OWASP) → [`agent-standards/70-security-owasp.md`](../../agent-standards/70-security-owasp.md)

Key reminders: keep endpoints thin (delegate to Application), bind request DTOs not entities, return
ProblemDetails on errors, correct status codes, flow the request `CancellationToken`, authorize by
default.
