using Microsoft.AspNetCore.Components.Authorization;

namespace BitzArt.Blazor.Auth;

public interface IPrerenderAuthenticationStateProvider
{
    public Task<AuthenticationState> GetPrerenderAuthenticationStateAsync();
}