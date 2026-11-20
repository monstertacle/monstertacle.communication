using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModelContextProtocol.AspNetCore;

namespace Monstertacle.Communication.Mcp;

/// <summary>Registers and maps the Streamable HTTP transport for host-defined MCP tools.</summary>
public static class McpTransportExtensions
{
    /// <summary>Adds stateless Streamable HTTP and the request-scoped principal context.</summary>
    public static IMcpServerBuilder AddMonstertacleMcp(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.TryAddScoped(provider =>
        {
            var httpContext = provider.GetRequiredService<IHttpContextAccessor>().HttpContext
                ?? throw new InvalidOperationException("MCP request context requires an active HTTP request.");

            return new McpRequestContext(httpContext.User);
        });

        return services.AddMcpServer()
            .WithHttpTransport(options => options.SessionMode = HttpServerSessionMode.Stateless);
    }

    /// <summary>Maps MCP at the supplied route so the host can apply endpoint authorization.</summary>
    public static IEndpointConventionBuilder MapMonstertacleMcp(
        this IEndpointRouteBuilder endpoints,
        string pattern)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(pattern);
        return endpoints.MapMcp(pattern);
    }
}
