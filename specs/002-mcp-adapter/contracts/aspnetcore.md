# ASP.NET Core integration contract

- `AddMonstertacleMcp(IServiceCollection)` adds stateless Streamable HTTP and
  returns the SDK tool builder so the host can register typed tool methods.
- `MapMonstertacleMcp(IEndpointRouteBuilder, string)` maps the supplied route
  and returns its convention builder for the host's authorization policy.
- `McpRequestContext` is available to tools via request dependency injection.
  Its principal comes from the current ASP.NET Core request. Client arguments
  do not establish identity. Tools receive cancellation through SDK binding.
- The host defines typed arguments and result projections, including any
  operation identifiers, uncertainty, or cancellation intent it exposes.
  Repeated requests are dispatched independently. The host owns deduplication
  and recovery.
