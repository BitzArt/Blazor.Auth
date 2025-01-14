using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace BitzArt.Blazor.Auth.Client;

public class WasmAuthenticationStateProvider(
    ILoggerFactory loggerFactory)
    : AuthenticationStateProvider
{
    private readonly ILogger _logger = loggerFactory.CreateLogger("Blazor.Auth.Client");

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        throw new NotImplementedException();
    }
}