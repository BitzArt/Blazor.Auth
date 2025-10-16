using Microsoft.AspNetCore.Components.Authorization;

namespace BitzArt.Blazor.Auth;

/// <summary>
/// A service responsible for authentication functionality.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Resolves the current User's authentication state.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task<AuthenticationState> GetAuthenticationStateAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Refresh the current User's JWT pair.
    /// </summary>
    /// <returns>An <see cref="AuthenticationOperationInfo"/> received
    /// after refreshing the User's JWT Pair and updating the Authentication State.</returns>
    public Task<AuthenticationOperationInfo> RefreshJwtPairAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Refresh the current User's JWT pair.
    /// </summary>
    /// <returns>An <see cref="AuthenticationOperationInfo"/> received
    /// after refreshing the User's JWT Pair and updating the Authentication State.</returns>
    [Obsolete("Use RefreshJwtPairAsync overload without refreshToken parameter instead.")]
    public Task<AuthenticationOperationInfo> RefreshJwtPairAsync(string refreshToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sign the current User out.
    /// </summary>
    /// <returns> A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task SignOutAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// A service responsible for authentication functionality.
/// </summary>
/// <typeparam name="TSignInPayload">The type of the sign-in payload.</typeparam>
public interface IUserService<TSignInPayload> : IUserService
{
    /// <summary>
    /// Sign the current User in.
    /// </summary>
    /// <returns>An <see cref="AuthenticationOperationInfo"/> received
    /// after signing the User in and updating the Authentication State.</returns>
    public Task<AuthenticationOperationInfo> SignInAsync(TSignInPayload signInPayload, CancellationToken cancellationToken = default);
}

/// <summary>
/// A service responsible for authentication functionality.
/// </summary>
/// <typeparam name="TSignInPayload">The type of the sign-in payload.</typeparam>
/// <typeparam name="TSignUpPayload">The type of the sign-up payload.</typeparam>
public interface IUserService<TSignInPayload, TSignUpPayload> : IUserService<TSignInPayload>
{
    /// <summary>
    /// Sign the current User up.
    /// </summary>
    /// <returns>An <see cref="AuthenticationOperationInfo"/> received
    /// after signing the User up and updating the Authentication State.</returns>
    public Task<AuthenticationOperationInfo> SignUpAsync(TSignUpPayload signUpPayload, CancellationToken cancellationToken = default);
}
