﻿namespace BitzArt.Blazor.Auth;

/// <summary>
/// A service responsible for authentication functionality.
/// </summary>
public interface IUserService
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
    /// Sign the current User out.
    /// </summary>
    /// <returns> A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task SignOutAsync();
}
