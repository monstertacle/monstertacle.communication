# Implementation plan: MCP Streamable HTTP adapter

Communication currently contains specifications and no runtime project, public
API, persisted state or callers. Preserve the existing .NET 10 and .NET 11
scope. The .NET 11 Hosting MCP edge is a planned caller. Its sessions feature
assigns endpoint authorization and OAuth discovery to Hosting and durable
recovery to its broker. The edge currently exposes only a liveness route.
The versioned Hosting admission contracts remain within Hosting.

Use an independent packable ASP.NET Core library and the official C# MCP SDK.
Expose registration for host tool definitions and endpoint mapping compatible
with ASP.NET Core authorization conventions. Verify the actual transport using
the SDK client on loopback Kestrel, plus focused protocol rejection checks.
Run the same suite on native Linux and Windows for both frameworks. Document
and verify source-based consumption from the published commit.

Constitution check: protocol dependency stays at the adapter boundary. Caller
authority comes from the current host request. Stateless transport keeps
durable recovery and cancellation outcomes outside the adapter.
