using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace BitzArt.Blazor.Auth.Client;

// Client-side implementation of the user service.
internal class UserService(
    IBlazorAuthLogger logger,
    BlazorHostHttpClient hostClient
    ) : IUserService
{
    private protected BlazorHostHttpClient HostClient = hostClient;

    public async Task<AuthenticationState> GetAuthenticationStateAsync(CancellationToken cancellationToken = default)
    {
        logger.LogDebug("GetAuthenticationStateAsync was called.");

        var response = await HostClient.GetAsync<ClaimsPrincipalDto>("/_auth/me", cancellationToken);

        if (response is null) return new AuthenticationState(new ClaimsPrincipal());

        var principal = response.ToModel();

        logger.LogDebug("GetAuthenticationStateAsync completed");

        return new AuthenticationState(principal);
    }

    public async Task<AuthenticationResultInfo> RefreshJwtPairAsync(string refreshToken, CancellationToken cancellationToken = default)
        => await HostClient.PostAsync<AuthenticationResultInfo>("/_auth/refresh", refreshToken, cancellationToken);

    public async Task SignOutAsync(CancellationToken cancellationToken = default)
    {
        var response = await HostClient.PostAsync("/_auth/sign-out", cancellationToken);

        response.Validate();
    }
}

internal class UserService<TSignInPayload>(IBlazorAuthLogger logger, BlazorHostHttpClient hostClient)
    : UserService(logger, hostClient), IUserService<TSignInPayload>
{
    public async Task<AuthenticationResultInfo> SignInAsync(TSignInPayload signInPayload, CancellationToken cancellationToken = default)
        => await HostClient.PostAsync<AuthenticationResultInfo>("/_auth/sign-in", signInPayload!, cancellationToken);
}

internal class UserService<TSignInPayload, TSignUpPayload>(IBlazorAuthLogger logger, BlazorHostHttpClient hostClient)
    : UserService<TSignInPayload>(logger, hostClient), IUserService<TSignInPayload, TSignUpPayload>
{
    public async Task<AuthenticationResultInfo> SignUpAsync(TSignUpPayload signUpPayload, CancellationToken cancellationToken = default)
        => await HostClient.PostAsync<AuthenticationResultInfo>("/_auth/sign-up", signUpPayload!, cancellationToken);
}
