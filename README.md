# Monstertacle.Communication

Communication adapters between applications and controlled agent execution.

## MCP Streamable HTTP

`Monstertacle.Communication.Mcp` exposes host-defined tools over Streamable
HTTP using the [official C# MCP SDK](https://github.com/modelcontextprotocol/csharp-sdk).
The adapter uses stateless transport. Each HTTP request gets the current
ASP.NET Core principal and request services.

Register typed tool methods and apply the host's existing authorization
policy to the endpoint.

```csharp
using Microsoft.Extensions.DependencyInjection;
using Monstertacle.Communication.Mcp;

builder.Services.AddMonstertacleMcp().WithTools<HostTools>();
// Configure the host's authentication scheme and authorization policy here.
var app = builder.Build();
app.MapMonstertacleMcp("/mcp").RequireAuthorization("mcp-access");
app.Run();
```

Define tool methods with the SDK's `[McpServerTool]` attribute. For example,
put this method on a non-static `HostTools` class.

```csharp
[McpServerTool(Name = "status", UseStructuredContent = true)]
public static Task<StatusResult> Status(
    Guid operationId,
    McpRequestContext request,
    IHostOperations operations,
    CancellationToken cancellationToken) =>
    operations.GetStatusAsync(operationId, request.Principal, cancellationToken);
```

The host defines `StatusResult` and registers `IHostOperations` in dependency
injection. Only `operationId` is a client tool argument. `McpRequestContext`
exposes the host's request principal, and the SDK supplies the cancellation
token. Authentication, OAuth discovery, authorization, admission,
idempotency, durable recovery and execution stay in the host. Disconnection
or cancellation signals that a caller stopped waiting. The host determines
whether external work actually stopped.

## Source consumption

Pin this repository to an immutable commit and reference
`src/Monstertacle.Communication.Mcp/Monstertacle.Communication.Mcp.csproj` from
the consuming ASP.NET Core project. The library targets .NET 10 and .NET 11.
To build a package for a local NuGet feed, use the pinned checkout.

```sh
dotnet pack src/Monstertacle.Communication.Mcp -c Release -o artifacts
```

Run the contract suite with the .NET 11 SDK and both ASP.NET Core runtimes
installed.

```sh
dotnet test -c Release
```

## Specification workflow

- [Project constitution](.specify/memory/constitution.md)
- [Runtime support scope](specs/001-runtime-support-scope/spec.md)
- [MCP adapter specification](specs/002-mcp-adapter/spec.md)
- [ASP.NET Core integration contract](specs/002-mcp-adapter/contracts/aspnetcore.md)

The specification scaffolding is based on Spec Kit 1.2.2. Shared Bash and
PowerShell helpers are in `.specify/scripts/`.
