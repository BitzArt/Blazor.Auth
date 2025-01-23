namespace BitzArt.Blazor.Auth.Client;

// Client-side implementation of the user service.
internal class UserService(BlazorHostHttpClient hostClient) : IUserService
{
    protected readonly BlazorHostHttpClient HostClient = hostClient;

    public async Task<AuthenticationResult> RefreshJwtPairAsync(string refreshToken)
        => await HostClient.PostAsync<AuthenticationResult>("/_auth/refresh", refreshToken);

    public async Task SignOutAsync()
    {
        var response = await HostClient.PostAsync("/_auth/sign-out");

        response.Validate();
    }
}

internal class UserService<TSignInPayload>(BlazorHostHttpClient hostClient)
    : UserService(hostClient), IUserService<TSignInPayload>
{
    public async Task<AuthenticationResult> SignInAsync(TSignInPayload signInPayload)
        => await HostClient.PostAsync<AuthenticationResult>("/_auth/sign-in", signInPayload!);
}

internal class UserService<TSignInPayload, TSignUpPayload>(BlazorHostHttpClient hostClient)
    : UserService<TSignInPayload>(hostClient), IUserService<TSignInPayload, TSignUpPayload>
{
    public async Task<AuthenticationResult> SignUpAsync(TSignUpPayload signUpPayload)
        => await HostClient.PostAsync<AuthenticationResult>("/_auth/sign-up", signUpPayload!);
}
