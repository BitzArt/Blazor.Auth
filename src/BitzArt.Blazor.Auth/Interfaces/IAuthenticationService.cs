namespace BitzArt.Blazor.Auth;

public interface IAuthenticationService
{
    public Task<AuthenticationResult?> SignInAsync(object signInPayload);
    public Task<AuthenticationResult?> SignUpAsync(object signUpPayload);
    public Task<AuthenticationResult?> RefreshAsync(string refreshToken);

    public Task<AuthenticationResult?> GetJwtPairAsync(object signInPayload);
    public Task<AuthenticationResult?> GetSignUpResultAsync(object signUpPayload);
    public Task<AuthenticationResult?> RefreshJwtPairAsync(string refreshToken);
    
    public Type? GetSignInPayloadType();
    public Type? GetSignUpPayloadType();
}
