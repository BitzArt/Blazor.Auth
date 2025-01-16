using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace BitzArt.Blazor.Auth.Client;

internal class ClientSideAuthenticationStateProvider(
    ClientSideLogger logger,
    BlazorHostClient hostClient)
    : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        logger.LogDebug("GetAuthenticationStateAsync was called.");

        var response = await hostClient.GetAsync<AuthenticationState>("/_auth/me");

        logger.LogDebug("GetAuthenticationStateAsync completed");
    }
}