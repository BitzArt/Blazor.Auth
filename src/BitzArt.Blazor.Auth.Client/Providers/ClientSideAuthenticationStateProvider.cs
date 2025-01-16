using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text.Json;

namespace BitzArt.Blazor.Auth.Client;

internal class ClientSideAuthenticationStateProvider(
    ClientSideLogger logger,
    BlazorHostHttpClient hostClient)
    : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        logger.LogDebug("GetAuthenticationStateAsync was called.");

        var response = await hostClient.GetAsync<string>("/_auth/me");

        var dto = JsonSerializer.Deserialize<ClaimsPrincipalDto>(response, Constants.JsonSerializerOptions);

        if (dto is null) return new AuthenticationState(new ClaimsPrincipal());

        var principal = dto.ToModel();

        logger.LogDebug("GetAuthenticationStateAsync completed");

        return new AuthenticationState(principal);
    }
}