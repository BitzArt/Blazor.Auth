using Microsoft.AspNetCore.Components.Authorization;

namespace BitzArt.Blazor.Auth;

/// <summary>
/// Options for configuring <b>Blazor.Auth</b>
/// </summary>
public class BlazorAuthOptions
{
    /// <summary>
    /// An option to disable automatic expiration handling.
    /// </summary>
    /// <remarks>
    /// <para>
    /// By default, the authentication state provider automatically handles token expiration
    /// by setting up a timer to refresh the authentication state when the token expires.
    /// </para>
    /// <para>
    /// Setting this option to <see langword="true"/> disables this automatic behavior, allowing
    /// for manual handling of token expiration.
    /// </para>
    /// <para>
    /// Default: <see langword="false"/>
    /// </para>
    /// </remarks>
    public bool DisableAutoExpirationHandling { get; set; } = false;

    /// <summary>
    /// An option to specify the initial dummy authentication state.
    /// </summary>
    /// <para>
    /// Specifying this option causes a dummy authentication state to be returned before performing time-consuming asynchronous authentication.
    /// This is effective for reducing screen flickering in Blazor Server Interactive.
    /// You can add a claim such as “loading” as needed to customize the component's display.
    /// </para>
    /// <para>
    /// Default: <see langword="null"/>
    /// </para>
    public AuthenticationState? InitialDummyAuthState { get; set; }
}
