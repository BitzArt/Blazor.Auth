using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BitzArt.Blazor.Auth.Client;

// Client-side implementation of the user service.
internal class UserService(BlazorHostHttpClient hostClient) : IUserService, IAuthStateUpdateNotifier
{
    private protected readonly BlazorHostHttpClient HostClient = hostClient;

    public event IAuthStateUpdateNotifier.AuthenticationStateUpdatedEventHandler? AuthenticationStateUpdated;

    protected void NotifyAuthenticationStateUpdated(AuthenticationOperationInfo? authInfo)
        => AuthenticationStateUpdated?.Invoke(this, authInfo);

    public async Task<AuthenticationState> GetAuthenticationStateAsync(CancellationToken cancellationToken = default)
    {
        var response = await HostClient.GetAsync<ClaimsPrincipalDto>("/_auth/me", cancellationToken);

        if (response is null) return new AuthenticationState(new ClaimsPrincipal());

        var principal = response.ToModel();

        return new AuthenticationState(principal);
    }

    public async Task<AuthenticationOperationInfo> RefreshJwtPairAsync(CancellationToken cancellationToken = default)
    {
        var result = await HostClient.PostAsync<AuthenticationOperationInfo>("/_auth/refresh", cancellationToken);

        NotifyAuthenticationStateUpdated(result);

        return result;
    }

    public async Task<AuthenticationOperationInfo> RefreshJwtPairAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var result = await HostClient.PostAsync<AuthenticationOperationInfo>("/_auth/refresh", refreshToken, cancellationToken);

        NotifyAuthenticationStateUpdated(result);

        return result;
    }

    public async Task SignOutAsync(CancellationToken cancellationToken = default)
    {
        var response = await HostClient.PostAsync("/_auth/sign-out", cancellationToken);

        NotifyAuthenticationStateUpdated(null);

        response.Validate();
    }
}

internal class UserService<TSignInPayload>(BlazorHostHttpClient hostClient)
    : UserService(hostClient), IUserService<TSignInPayload>
{
    public async Task<AuthenticationOperationInfo> SignInAsync(TSignInPayload signInPayload, CancellationToken cancellationToken = default)
    {
        var result = await HostClient.PostAsync<AuthenticationOperationInfo>("/_auth/sign-in", signInPayload!, cancellationToken);

        NotifyAuthenticationStateUpdated(result);

        return result;
    }
}

internal class UserService<TSignInPayload, TSignUpPayload>(BlazorHostHttpClient hostClient)
    : UserService<TSignInPayload>(hostClient), IUserService<TSignInPayload, TSignUpPayload>
{
    public async Task<AuthenticationOperationInfo> SignUpAsync(TSignUpPayload signUpPayload, CancellationToken cancellationToken = default)
    {
        var result = await HostClient.PostAsync<AuthenticationOperationInfo>("/_auth/sign-up", signUpPayload!, cancellationToken);

        NotifyAuthenticationStateUpdated(result);

        return result;
    }
}
