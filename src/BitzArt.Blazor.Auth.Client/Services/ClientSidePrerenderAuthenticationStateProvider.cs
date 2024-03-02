using Microsoft.AspNetCore.Components.Authorization;

namespace BitzArt.Blazor.Auth.Server.Services;

internal class ClientSidePrerenderAuthenticationStateProvider : IPrerenderAuthenticationStateProvider
{
    public Task<AuthenticationState> GetPrerenderAuthenticationStateAsync()
    {
        throw new NotImplementedException("Prerender logic is not available in client-side code.");
    }
}
