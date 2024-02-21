namespace BitzArt.Blazor.Auth;

public interface IAuthenticationService
{
    public Task<JwtPair?> SignInAsync(object signInPayload);
    public Task<JwtPair?> RefreshAsync(string refreshToken);
    public Type? GetSignInPayloadType();
    Task<JwtPair?> RefreshJwtPairAsync(string refreshToken);
    Task<JwtPair?> GetJwtPairAsync(object signInPayload);
}
