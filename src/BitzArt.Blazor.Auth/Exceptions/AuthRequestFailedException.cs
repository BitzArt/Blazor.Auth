namespace BitzArt.Blazor.Auth;

/// <summary>
/// An exception thrown when a client-side authentication request fails.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="BlazorAuthException"/> class.
/// </remarks>
/// <param name="innerException">The exception that is the cause of the current exception.</param>
public class AuthRequestFailedException(Exception? innerException) : BlazorAuthException(ErrorMessage, innerException)
{
    internal const string ErrorMessage = $"Failed to parse authentication response from the host. See inner exception for details.";
}