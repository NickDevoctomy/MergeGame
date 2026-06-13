# ASP.NET Core API Conventions

You are an ASP.NET Core API specialist. These rules apply to the presentation layer
(`**/Api/**`, `**/Web/**`).

## Endpoint design

- Prefer **Minimal APIs** grouped with `MapGroup` and organized by feature; controllers are
  acceptable for large, conventionally-routed surfaces. Be consistent within the project.
- Keep endpoints thin: validate input, delegate to an Application use case (handler/service), map the
  result, return. **No business logic or EF queries in endpoints.**
- Use attribute/explicit routing with consistent, lowercase, plural resource names
  (`/api/v1/orders`).
- Version the API (`/api/v{version}/...`).

## Requests & responses

- Bind to dedicated request DTOs (`record` types); never bind Domain entities.
- Return typed results (`Results<Ok<T>, NotFound, ValidationProblem>` or `ActionResult<T>`).
- Use correct status codes: `200/201/204` success, `400` validation, `401/403` auth, `404` missing,
  `409` conflict, `500` unexpected.
- Return errors as **RFC 7807 ProblemDetails**. Add a global exception handler
  (`IExceptionHandler` / `UseExceptionHandler`) so handlers don't leak stack traces.
- Validate input at the boundary (e.g., FluentValidation or minimal-API filters) and return
  `ValidationProblem` on failure.

## Cross-cutting

- Accept and flow the request `CancellationToken` into Application calls (see
  [`30-async.md`](30-async.md)).
- Configure DI in the composition root only; register Application/Infrastructure services via
  extension methods (`AddApplication()`, `AddInfrastructure(config)`).
- Enable health checks, structured logging, and OpenAPI/Swagger in non-production as appropriate.
- Apply security headers, authentication/authorization, rate limiting, and CORS deliberately — see
  [`70-security-owasp.md`](70-security-owasp.md).

<!-- PROJECT-SPECIFIC OVERRIDES
e.g., auth scheme (JWT/OIDC), API versioning library, pagination envelope shape, error codes.
-->
