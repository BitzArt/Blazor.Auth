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
}
