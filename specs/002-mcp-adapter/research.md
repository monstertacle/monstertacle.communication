# Transport and compatibility

Use the [official C# SDK](https://github.com/modelcontextprotocol/csharp-sdk)
ASP.NET Core transport in stateless mode. It handles the MCP wire protocol
and gives each request its own service scope. Built-in tool registration
preserves host-selected typed tool contracts and allows ordinary ASP.NET Core
authentication and endpoint authorization to run before dispatch.

Stateful transport would bind clients to a transport session that the planned
Hosting design does not need. Its durable operation UUIDs belong to the
application contract and are unaffected by MCP protocol negotiation.
