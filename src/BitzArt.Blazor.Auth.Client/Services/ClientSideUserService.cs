namespace BitzArt.Blazor.Auth.Client;

internal class ClientSideUserService(BlazorHostHttpClient hostClient) : IUserService
{
    public async Task<AuthenticationResult> SignInAsync(object signInPayload)
        => await hostClient.PostAsync<AuthenticationResult>("/_auth/sign-in", signInPayload);

    public async Task<AuthenticationResult> SignUpAsync(object signUpPayload)
        => await hostClient.PostAsync<AuthenticationResult>("/_auth/sign-up", signUpPayload);

    public async Task<AuthenticationResult> RefreshJwtPairAsync(string refreshToken)
        => await hostClient.PostAsync<AuthenticationResult>("/_auth/refresh", refreshToken);

    public async Task SignOutAsync()
    {
        var response = await hostClient.PostAsync("/_auth/sign-out");

        response.Validate();
    }
}
