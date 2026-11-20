# Feature 002: MCP Streamable HTTP adapter

A consuming ASP.NET Core host can expose typed tools through standard MCP
Streamable HTTP. An MCP client can negotiate a supported protocol, discover
host-registered tools and call them. The host supplies the tool names, input
and output contracts, operation handlers and endpoint authorization policy.

- FR-001: Use the official C# MCP SDK for protocol negotiation, discovery,
  argument binding, response serialization and Streamable HTTP.
- FR-002: Make transport requests independent. Each call resolves the current
  HTTP request principal and scoped services, even after a client reconnects.
- FR-003: Preserve each request's cancellation token and the handler's result.
  The adapter does not retry calls or infer work completion from cancellation.
- FR-004: Keep OAuth, credentials, admission, replay, durable state, execution
  and operation-level authorization in the consuming host.
- FR-005: Target .NET 10 and .NET 11 and verify contracts on native Linux and
  Windows before claiming platform support or releasing a package.

Acceptance requires credential-free tests of discovery, typed invocation,
malformed arguments, host rejection before dispatch, separate principals,
cancellation, repeated calls and recovery with a fresh MCP client.
