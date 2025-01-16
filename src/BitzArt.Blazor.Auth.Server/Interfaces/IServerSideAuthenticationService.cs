namespace BitzArt.Blazor.Auth.Server;

/// <summary>
/// A service that provides methods for server-side authentication.
/// </summary>
public interface IServerSideAuthenticationService
{
    /// <summary>
    /// Sign the current User in.
    /// </summary>
    /// <returns>An <see cref="AuthenticationResult"/> received
    /// after signing the User in and updating the Authentication State.</returns>
    public Task<AuthenticationResult> SignInAsync(object signInPayload);

    /// <summary>
    /// Sign the current User up.
    /// </summary>
    /// <returns>An <see cref="AuthenticationResult"/> received
    /// after signing the User up and updating the Authentication State.</returns>
    public Task<AuthenticationResult> SignUpAsync(object signUpPayload);

    /// <summary>
    /// Refresh the current User's JWT pair.
    /// </summary>
    /// <returns>An <see cref="AuthenticationResult"/> received
    /// after refreshing the User's JWT Pair and updating the Authentication State.</returns>
    public Task<AuthenticationResult> RefreshJwtPairAsync(string refreshToken);

    /// <summary>
    /// Get the type of the sign-in payload.
    /// </summary>
    /// <returns> The type of the sign-in payload.</returns>
    public Type? GetSignInPayloadType();

    /// <summary>
    /// Get the type of the sign-up payload.
    /// </summary>
    /// <returns> The type of the sign-up payload.</returns>
    public Type? GetSignUpPayloadType();
}
