namespace BitzArt.Blazor.Auth.Server;

/// <summary>
/// Options for the Blazor Auth Server.
/// </summary>
public class BlazorAuthServerOptions : BlazorAuthOptions
{
    /// <summary>
    /// Allows the app to operate in a non-HTTPS environment.
    /// </summary>
    public bool DisableSecureCookieFlag { get; set; } = false;
}
