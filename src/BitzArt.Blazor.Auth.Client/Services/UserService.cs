using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text.Json;

namespace BitzArt.Blazor.Auth.Client;

// Client-side implementation of the user service.
internal class UserService(
    IBlazorAuthLogger logger,
    BlazorHostHttpClient hostClient
    ) : IUserService
{
    private protected BlazorHostHttpClient HostClient = hostClient;

    public async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        logger.LogDebug("GetAuthenticationStateAsync was called.");

        var response = await HostClient.GetAsync<string>("/_auth/me");

        var dto = JsonSerializer.Deserialize<ClaimsPrincipalDto>(response, Constants.JsonSerializerOptions);

        if (dto is null) return new AuthenticationState(new ClaimsPrincipal());

        var principal = dto.ToModel();

        logger.LogDebug("GetAuthenticationStateAsync completed");

        return new AuthenticationState(principal);
    }

    public async Task<AuthenticationResultInfo> RefreshJwtPairAsync(string refreshToken)
        => await HostClient.PostAsync<AuthenticationResultInfo>("/_auth/refresh", refreshToken);

    public async Task SignOutAsync()
    {
        var response = await HostClient.PostAsync("/_auth/sign-out");

        response.Validate();
    }
}

internal class UserService<TSignInPayload>(IBlazorAuthLogger logger, BlazorHostHttpClient hostClient)
    : UserService(logger, hostClient), IUserService<TSignInPayload>
{
    public async Task<AuthenticationResultInfo> SignInAsync(TSignInPayload signInPayload)
        => await HostClient.PostAsync<AuthenticationResultInfo>("/_auth/sign-in", signInPayload!);
}

internal class UserService<TSignInPayload, TSignUpPayload>(IBlazorAuthLogger logger, BlazorHostHttpClient hostClient)
    : UserService<TSignInPayload>(logger, hostClient), IUserService<TSignInPayload, TSignUpPayload>
{
    public async Task<AuthenticationResultInfo> SignUpAsync(TSignUpPayload signUpPayload)
        => await HostClient.PostAsync<AuthenticationResultInfo>("/_auth/sign-up", signUpPayload!);
}
