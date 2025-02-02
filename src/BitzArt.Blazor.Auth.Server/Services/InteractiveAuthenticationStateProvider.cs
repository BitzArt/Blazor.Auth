using Microsoft.AspNetCore.Components.Authorization;

namespace BitzArt.Blazor.Auth.Server;

internal class InteractiveAuthenticationStateProvider
    : BlazorAuthAuthenticationStateProvider
{
    private protected override async Task<AuthenticationState> ResolveAuthenticationStateAsync()
    {
        await Task.Delay(0);
        throw new NotImplementedException();
    }
}