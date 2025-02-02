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
    public Task<AuthenticationState> GetAuthenticationStateAsync();

    /// <summary>
    /// Refresh the current User's JWT pair.
    /// </summary>
    /// <returns>An <see cref="AuthenticationResultInfo"/> received
    /// after refreshing the User's JWT Pair and updating the Authentication State.</returns>
    public Task<AuthenticationResultInfo> RefreshJwtPairAsync(string refreshToken);

    /// <summary>
    /// Sign the current User out.
    /// </summary>
    /// <returns> A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task SignOutAsync();
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
    /// <returns>An <see cref="AuthenticationResultInfo"/> received
    /// after signing the User in and updating the Authentication State.</returns>
    public Task<AuthenticationResultInfo> SignInAsync(TSignInPayload signInPayload);
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
    /// <returns>An <see cref="AuthenticationResultInfo"/> received
    /// after signing the User up and updating the Authentication State.</returns>
    public Task<AuthenticationResultInfo> SignUpAsync(TSignUpPayload signUpPayload);
}
