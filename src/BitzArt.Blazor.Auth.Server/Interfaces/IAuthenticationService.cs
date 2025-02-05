namespace BitzArt.Blazor.Auth.Server;

/// <summary>
/// A service that provides server-side methods for user authentication.
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Refresh the current User's JWT pair.
    /// </summary>
    /// <param name="refreshToken">The refresh token.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> containing the <see cref="AuthenticationResult"/>
    /// received after refreshing the User's JWT Pair.</returns>
    public Task<AuthenticationResult> RefreshJwtPairAsync(string refreshToken, CancellationToken cancellationToken = default);
}

/// <summary>
/// A service that provides server-side methods for user authentication.
/// </summary>
/// <typeparam name="TSignInPayload">The type of the sign-in payload.</typeparam>
public interface IAuthenticationService<TSignInPayload> : IAuthenticationService
{
    /// <summary>
    /// Sign the current User in.
    /// </summary>
    /// <param name="signInPayload">The sign-in payload.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> containing the <see cref="AuthenticationResult"/>
    /// received after signing the User in.</returns>
    public Task<AuthenticationResult> SignInAsync(TSignInPayload signInPayload, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the type of the sign-in payload.
    /// </summary>
    /// <returns>The type of the sign-in payload.</returns>
    public Type? GetSignInPayloadType();
}

/// <summary>
/// A service that provides server-side methods for user authentication.
/// </summary>
/// <typeparam name="TSignInPayload">The type of the sign-in payload.</typeparam>
/// <typeparam name="TSignUpPayload">The type of the sign-up payload.</typeparam>
public interface IAuthenticationService<TSignInPayload, TSignUpPayload> : IAuthenticationService<TSignInPayload>
{
    /// <summary>
    /// Sign the current User up.
    /// </summary>
    /// <param name="signUpPayload">The sign-up payload.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> containing the <see cref="AuthenticationResult"/>
    /// received after signing the User up.</returns>
    public Task<AuthenticationResult> SignUpAsync(TSignUpPayload signUpPayload, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the type of the sign-up payload.
    /// </summary>
    /// <returns>The type of the sign-up payload.</returns>
    public Type? GetSignUpPayloadType();
}
