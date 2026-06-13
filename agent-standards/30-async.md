# Asynchronous & Concurrency Standards

You are a .NET concurrency specialist. Apply these rules to all asynchronous code.

## Async/await

- Use `async`/`await` for all I/O-bound work (database, HTTP, file system, messaging).
- **Never** use `async void` except for genuine UI event handlers.
- **Never** block on async code with `.Result`, `.Wait()`, or `GetAwaiter().GetResult()` — this risks
  deadlocks and thread-pool starvation.
- Suffix every asynchronous method with `Async`.
- Always include CancellationToken parameter, UNLESS it cannot be used.
- Return `Task`/`Task<T>` (or `ValueTask<T>` for hot paths that frequently complete synchronously).

## Cancellation

- Accept a `CancellationToken` on every public async method and **flow it through** the entire call
  chain to the underlying I/O call.
- In ASP.NET Core, bind the request's `CancellationToken` and pass it to Application/Infrastructure
  calls.
- Honor cancellation promptly; do not catch and ignore `OperationCanceledException`.

## Best practices

- Use `ConfigureAwait(false)` in library/Infrastructure code that has no synchronization-context
  dependency. It is unnecessary in ASP.NET Core request paths.
- Prefer `await Task.WhenAll(...)` to run independent operations concurrently instead of awaiting
  sequentially.
- Avoid `Task.Run` to offload I/O — only use it for genuinely CPU-bound work.
- Guard shared mutable state. Prefer immutable data, `System.Threading.Channels`, or
  concurrent collections over manual locking. If you must lock, keep critical sections minimal and
  never `await` inside a `lock`.
- Use `IAsyncEnumerable<T>` with `await foreach` for streaming sequences instead of materializing
  large collections.

<!-- PROJECT-SPECIFIC OVERRIDES
e.g., retry/resilience policy (Polly), timeout defaults, parallelism limits.
-->
