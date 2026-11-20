using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Client;
using ModelContextProtocol.Server;

namespace Monstertacle.Communication.Mcp.Tests;

internal sealed class LoopbackHost(WebApplication app, ToolProbe probe) : IAsyncDisposable
{
    private const string CallerHeader = "Test-Caller";
    private const string Scheme = "TestCaller";
    private Uri Endpoint => new(new Uri(app.Urls.Single()), "/tools");
    internal ToolProbe Probe => probe;

    internal static async Task<LoopbackHost> StartAsync()
    {
        var builder = WebApplication.CreateSlimBuilder();
        builder.Logging.ClearProviders();
        builder.WebHost.ConfigureKestrel(options => options.Listen(IPAddress.Loopback, 0));
        builder.Services.AddAuthentication(Scheme).AddScheme<AuthenticationSchemeOptions, TestAuthentication>(Scheme, null);
        builder.Services.AddAuthorizationBuilder().AddPolicy("tools", policy => policy
            .RequireAuthenticatedUser()
            .RequireClaim(ClaimTypes.Name, "alice", "bob"));
        var probe = new ToolProbe();
        builder.Services.AddSingleton(probe);
        builder.Services.AddMonstertacleMcp().WithTools<ProbeTools>();
        var app = builder.Build();
        app.MapMonstertacleMcp("/tools").RequireAuthorization("tools");
        await app.StartAsync();
        return new LoopbackHost(app, probe);
    }

    internal Task<McpClient> ConnectAsync(string caller, CancellationToken cancellationToken, string? protocol = null)
    {
        var transport = new HttpClientTransport(new HttpClientTransportOptions
        {
            Endpoint = Endpoint,
            TransportMode = HttpTransportMode.StreamableHttp,
            AdditionalHeaders = new Dictionary<string, string> { [CallerHeader] = caller }
        });
        return McpClient.CreateAsync(transport, new McpClientOptions { ProtocolVersion = protocol },
            cancellationToken: cancellationToken);
    }

    internal async Task<HttpResponseMessage> PostAsync(
        HttpClient http, string? caller, string payload, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, Endpoint);
        request.Headers.TryAddWithoutValidation("Accept", "application/json, text/event-stream");
        if (caller is not null)
        {
            request.Headers.Add(CallerHeader, caller);
        }

        request.Content = new StringContent(payload, Encoding.UTF8, "application/json");
        return await http.SendAsync(request, cancellationToken);
    }

    public ValueTask DisposeAsync() => app.DisposeAsync();

    private sealed class TestAuthentication(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
    {
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(CallerHeader, out var caller))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var principal = new ClaimsPrincipal(
                new ClaimsIdentity([new Claim(ClaimTypes.Name, caller.ToString())], Scheme.Name));
            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name)));
        }
    }
}

internal sealed class ToolProbe
{
    private int calls;

    internal int Calls => Volatile.Read(ref calls);
    internal TaskCompletionSource Started { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
    internal TaskCompletionSource Cancelled { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

    internal int RecordCall() => Interlocked.Increment(ref calls);
}

internal sealed class ProbeTools
{
    [McpServerTool(Name = "observe", UseStructuredContent = true)]
    public static Observation Observe(Guid operationId, McpRequestContext context, ToolProbe probe)
    {
        return new Observation(operationId, context.Principal.Identity!.Name!, probe.RecordCall());
    }

    [McpServerTool(Name = "interrupt")]
    public static async Task<string> Interrupt(ToolProbe probe, CancellationToken cancellationToken)
    {
        probe.Started.SetResult();
        try
        {
            await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            return "completed";
        }
        finally
        {
            if (cancellationToken.IsCancellationRequested)
            {
                probe.Cancelled.TrySetResult();
            }
        }
    }
}

internal sealed record Observation(Guid OperationId, string Caller, int Dispatch);
