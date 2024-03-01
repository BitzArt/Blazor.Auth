namespace BitzArt.Blazor.Auth;

/// <summary>
/// A service responsible for authentication functionality.
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Process a sign-in payload and return an <see cref="AuthenticationResult"/>.
    /// </summary>
    /// <returns>An <see cref="AuthenticationResult"/> created
    /// when signing the User in.</returns>
    public Task<AuthenticationResult> SignInAsync(object signInPayload);

    /// <summary>
    /// Process a sign-up payload and return an <see cref="AuthenticationResult"/>.
    /// </summary>
    /// <returns>An <see cref="AuthenticationResult"/> created
    /// when signing the User up.</returns>
    public Task<AuthenticationResult> SignUpAsync(object signUpPayload);

    /// <summary>
    /// Process a refresh payload and return an <see cref="AuthenticationResult"/>.
    /// </summary>
    /// <returns>An <see cref="AuthenticationResult"/> created
    /// when refreshing the User's .</returns>
    public Task<AuthenticationResult> RefreshJwtPairAsync(string refreshToken);
}