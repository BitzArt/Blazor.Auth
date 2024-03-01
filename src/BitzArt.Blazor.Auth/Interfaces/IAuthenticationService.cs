namespace BitzArt.Blazor.Auth;

public interface IAuthenticationService
{
    public Task<AuthenticationResult?> SignInAsync(object signInPayload);
    public Task<AuthenticationResult?> SignUpAsync(object signUpPayload);
    public Task<AuthenticationResult?> RefreshJetPairAsync(string refreshToken);

    public Task<AuthenticationResult?> GetSignInResultAsync(object signInPayload);
    public Task<AuthenticationResult?> GetSignUpResultAsync(object signUpPayload);
    public Task<AuthenticationResult?> GetRefreshJwtPairResultAsync(string refreshToken);

    public Type? GetSignInPayloadType();
    public Type? GetSignUpPayloadType();
}
