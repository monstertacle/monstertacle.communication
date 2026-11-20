using System.Security.Claims;

namespace Monstertacle.Communication.Mcp;

/// <summary>Identity supplied by ASP.NET Core for the current MCP HTTP request.</summary>
public sealed class McpRequestContext
{
    internal McpRequestContext(ClaimsPrincipal principal)
    {
        Principal = principal;
    }

    /// <summary>The principal established by the host for this request.</summary>
    public ClaimsPrincipal Principal { get; }
}
