namespace BitzArt.Blazor.Auth.Client;

// Client-side implementation of the user service.
internal class UserService(BlazorHostHttpClient hostClient) : IUserService
{
    protected readonly BlazorHostHttpClient HostClient = hostClient;

    public async Task<AuthenticationResultInfo> RefreshJwtPairAsync(string refreshToken)
        => await HostClient.PostAsync<AuthenticationResultInfo>("/_auth/refresh", refreshToken);

    public async Task SignOutAsync()
    {
        var response = await HostClient.PostAsync("/_auth/sign-out");

        response.Validate();
    }
}

internal class UserService<TSignInPayload>(BlazorHostHttpClient hostClient)
    : UserService(hostClient), IUserService<TSignInPayload>
{
    public async Task<AuthenticationResultInfo> SignInAsync(TSignInPayload signInPayload)
        => await HostClient.PostAsync<AuthenticationResultInfo>("/_auth/sign-in", signInPayload!);
}

internal class UserService<TSignInPayload, TSignUpPayload>(BlazorHostHttpClient hostClient)
    : UserService<TSignInPayload>(hostClient), IUserService<TSignInPayload, TSignUpPayload>
{
    public async Task<AuthenticationResultInfo> SignUpAsync(TSignUpPayload signUpPayload)
        => await HostClient.PostAsync<AuthenticationResultInfo>("/_auth/sign-up", signUpPayload!);
}
