namespace BitzArt.Blazor.Auth.Server;

/// <inheritdoc cref="IAuthenticationService"/>
public abstract class AuthenticationService : IAuthenticationService
{
    /// <summary>
    /// Refresh the current User's JWT pair.
    /// </summary>
    /// <param name="refreshToken">The refresh token.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> containing the <see cref="AuthenticationResult"/>
    /// received after refreshing the User's JWT Pair.</returns>
    public abstract Task<AuthenticationResult> RefreshJwtPairAsync(string refreshToken, CancellationToken cancellationToken = default);

    /// <inheritdoc cref="AuthenticationResult.Success(JwtPair, IDictionary{string, object}?)"/>
    protected static AuthenticationResult Success(JwtPair jwtPair, IDictionary<string, object>? data = null)
        => AuthenticationResult.Success(jwtPair, data);

    /// <inheritdoc cref="AuthenticationResult.Failure(string, IDictionary{string, object}?)"/>
    protected static AuthenticationResult Failure(string errorMessage)
        => AuthenticationResult.Failure(errorMessage);
}

/// <inheritdoc cref="IAuthenticationService{TSignInPayload}"/>
public abstract class AuthenticationService<TSignInPayload> : AuthenticationService, IAuthenticationService<TSignInPayload>
{
    /// <summary>
    /// Signs the User in.
    /// </summary>
    /// <param name="signInPayload">The sign-in payload.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> containing the <see cref="AuthenticationResult"/> received after signing the User in.</returns>
    public abstract Task<AuthenticationResult> SignInAsync(TSignInPayload signInPayload, CancellationToken cancellationToken = default);

    Type? IAuthenticationService<TSignInPayload>.GetSignInPayloadType() => typeof(TSignInPayload);
}

/// <summary>
/// A service that provides methods for server-side authentication.
/// </summary>
/// <typeparam name="TSignInPayload">The type of the sign-in payload.</typeparam>
/// <typeparam name="TSignUpPayload">The type of the sign-up payload.</typeparam>
public abstract class AuthenticationService<TSignInPayload, TSignUpPayload> : AuthenticationService<TSignInPayload>, IAuthenticationService<TSignInPayload, TSignUpPayload>
{
    /// <summary>
    /// Signs the User up.
    /// </summary>
    /// <param name="signUpPayload">The sign-up payload.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> containing the <see cref="AuthenticationResult"/> received after signing the User up.</returns>
    public abstract Task<AuthenticationResult> SignUpAsync(TSignUpPayload signUpPayload, CancellationToken cancellationToken = default);

    Type? IAuthenticationService<TSignInPayload, TSignUpPayload>.GetSignUpPayloadType() => typeof(TSignUpPayload);
}
