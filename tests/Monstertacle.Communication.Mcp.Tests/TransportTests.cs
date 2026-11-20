using System.Net;
using System.Text.Json;
using ModelContextProtocol.Client;

namespace Monstertacle.Communication.Mcp.Tests;

public sealed class TransportTests
{
    private static readonly Guid OperationId = Guid.Parse("d879f065-b876-43b7-b12c-87dba29360b2");
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    [Theory]
    [InlineData("2025-11-25")]
    [InlineData("2026-07-28")]
    public async Task DiscoversAndInvokesTypedTool(string protocol)
    {
        using var timeout = NewTimeout();
        await using var host = await LoopbackHost.StartAsync();
        await using var client = await host.ConnectAsync("alice", timeout.Token, protocol);
        var tools = await client.ListToolsAsync(cancellationToken: timeout.Token);
        var tool = Assert.Single(tools, tool => tool.Name == "observe");
        Assert.Equal("operationId", Assert.Single(tool.JsonSchema.GetProperty("properties").EnumerateObject()).Name);
        Assert.Equal(new Observation(OperationId, "alice", 1), await ObserveAsync(client, timeout.Token));
    }

    [Theory]
    [InlineData("""{"jsonrpc":"2.0","id":1,"method":"initialize","params":{"protocolVersion":"2025-11-25","capabilities":{},"clientInfo":{"name":"probe","version":"1"}}}""")]
    [InlineData("""{"jsonrpc":"2.0","id":1,"method":"tools/list","params":{}}""")]
    [InlineData("""{"jsonrpc":"2.0","id":1,"method":"tools/call","params":{"name":"observe","arguments":{"operationId":"d879f065-b876-43b7-b12c-87dba29360b2"}}}""")]
    public async Task HostAuthorizationRejectsMissingAndUnlistedCallers(string payload)
    {
        using var timeout = NewTimeout();
        await using var host = await LoopbackHost.StartAsync();
        using var http = new HttpClient();
        using var anonymous = await host.PostAsync(http, null, payload, timeout.Token);
        using var unlisted = await host.PostAsync(http, "mallory", payload, timeout.Token);
        Assert.Equal(HttpStatusCode.Unauthorized, anonymous.StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, unlisted.StatusCode);
        Assert.Equal(0, host.Probe.Calls);
    }

    [Fact]
    public async Task RejectsMalformedTypedArgumentBeforeDispatch()
    {
        using var timeout = NewTimeout();
        await using var host = await LoopbackHost.StartAsync();
        await using var client = await host.ConnectAsync("alice", timeout.Token);
        var result = await client.CallToolAsync("observe",
            new Dictionary<string, object?> { ["operationId"] = "invalid" }, cancellationToken: timeout.Token);
        Assert.True(result.IsError);
        Assert.Equal(0, host.Probe.Calls);
    }

    [Fact]
    public async Task FreshClientAndSecondIdentityUseIndependentRequests()
    {
        using var timeout = NewTimeout();
        await using var host = await LoopbackHost.StartAsync();
        await using (var first = await host.ConnectAsync("alice", timeout.Token))
        {
            Assert.Equal(new Observation(OperationId, "alice", 1), await ObserveAsync(first, timeout.Token));
        }

        await using var second = await host.ConnectAsync("bob", timeout.Token);
        await using var fresh = await host.ConnectAsync("alice", timeout.Token);
        Assert.Equal(1, host.Probe.Calls);
        Assert.Equal(new Observation(OperationId, "bob", 2), await ObserveAsync(second, timeout.Token));
        Assert.Equal(new Observation(OperationId, "alice", 3), await ObserveAsync(fresh, timeout.Token));
        Assert.Equal(new Observation(OperationId, "alice", 4), await ObserveAsync(fresh, timeout.Token));
    }

    [Fact]
    public async Task ClientCancellationReachesToolAndFreshClientCanCall()
    {
        using var timeout = NewTimeout();
        await using var host = await LoopbackHost.StartAsync();
        await using var client = await host.ConnectAsync("alice", timeout.Token);
        using var cancelCall = CancellationTokenSource.CreateLinkedTokenSource(timeout.Token);
        var call = client.CallToolAsync("interrupt", cancellationToken: cancelCall.Token);
        await host.Probe.Started.Task.WaitAsync(timeout.Token);
        cancelCall.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await call);
        await host.Probe.Cancelled.Task.WaitAsync(timeout.Token);

        await using var fresh = await host.ConnectAsync("alice", timeout.Token);
        Assert.Equal(new Observation(OperationId, "alice", 1), await ObserveAsync(fresh, timeout.Token));
    }

    private static CancellationTokenSource NewTimeout() => new(TimeSpan.FromSeconds(15));

    private static async Task<Observation?> ObserveAsync(McpClient client, CancellationToken cancellationToken)
    {
        var result = await client.CallToolAsync("observe",
            new Dictionary<string, object?> { ["operationId"] = OperationId }, cancellationToken: cancellationToken);
        Assert.NotEqual(true, result.IsError);
        var structured = Assert.IsType<JsonElement>(result.StructuredContent);
        return structured.Deserialize<Observation>(JsonOptions);
    }
}
