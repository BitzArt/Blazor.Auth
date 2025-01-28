using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BitzArt.Blazor.Auth;

/// <inheritdoc/>
internal abstract class BlazorAuthAuthenticationStateProvider : AuthenticationStateProvider
{
    private protected static AuthenticationState UnauthorizedState => new(new ClaimsPrincipal());

    /// <inheritdoc/>
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var task = ResolveAuthenticationStateAsync();

        NotifyAuthenticationStateChanged(task);

        return await task;
    }

    private protected abstract Task<AuthenticationState> ResolveAuthenticationStateAsync();
}
