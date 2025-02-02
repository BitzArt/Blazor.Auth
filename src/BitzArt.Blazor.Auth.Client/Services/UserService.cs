using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BitzArt.Blazor.Auth.Client;

// Client-side implementation of the user service.
internal class UserService(BlazorHostHttpClient hostClient) : IUserService
{
    private protected readonly BlazorHostHttpClient HostClient = hostClient;

    public async Task<AuthenticationState> GetAuthenticationStateAsync(CancellationToken cancellationToken = default)
    {
        var response = await HostClient.GetAsync<ClaimsPrincipalDto>("/_auth/me", cancellationToken);

        if (response is null) return new AuthenticationState(new ClaimsPrincipal());

        var principal = response.ToModel();

        return new AuthenticationState(principal);
    }

    public async Task<AuthenticationResultInfo> RefreshJwtPairAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var result = await HostClient.PostAsync<AuthenticationResultInfo>("/_auth/refresh", refreshToken, cancellationToken);

        return result;
    }

    public async Task SignOutAsync(CancellationToken cancellationToken = default)
    {
        var response = await HostClient.PostAsync("/_auth/sign-out", cancellationToken);

        response.Validate();
    }
}

internal class UserService<TSignInPayload>(BlazorHostHttpClient hostClient)
    : UserService(hostClient), IUserService<TSignInPayload>
{
    public async Task<AuthenticationResultInfo> SignInAsync(TSignInPayload signInPayload, CancellationToken cancellationToken = default)
    {
        var result = await HostClient.PostAsync<AuthenticationResultInfo>("/_auth/sign-in", signInPayload!, cancellationToken);

        return result;
    }
}

internal class UserService<TSignInPayload, TSignUpPayload>(BlazorHostHttpClient hostClient)
    : UserService<TSignInPayload>(hostClient), IUserService<TSignInPayload, TSignUpPayload>
{
    public async Task<AuthenticationResultInfo> SignUpAsync(TSignUpPayload signUpPayload, CancellationToken cancellationToken = default)
    {
        var result = await HostClient.PostAsync<AuthenticationResultInfo>("/_auth/sign-up", signUpPayload!, cancellationToken);

        return result;
    }
}
