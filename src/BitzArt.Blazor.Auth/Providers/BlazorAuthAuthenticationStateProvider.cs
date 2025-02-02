using Microsoft.AspNetCore.Components.Authorization;

namespace BitzArt.Blazor.Auth;

/// <inheritdoc/>
internal class BlazorAuthAuthenticationStateProvider(IUserService userService) : AuthenticationStateProvider
{
    /// <inheritdoc/>
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var task = userService.GetAuthenticationStateAsync();

        NotifyAuthenticationStateChanged(task);

        return await task;
    }
}
