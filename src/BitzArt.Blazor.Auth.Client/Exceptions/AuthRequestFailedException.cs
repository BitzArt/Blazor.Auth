namespace BitzArt.Blazor.Auth.Client;

/// <summary>
/// An exception thrown when a client-side authentication request fails.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="BlazorAuthException"/> class.
/// </remarks>
/// <param name="message">The error message that explains the reason for the exception.</param>
/// <param name="innerException">The exception that is the cause of the current exception.</param>
public class AuthRequestFailedException(string message, Exception? innerException) : BlazorAuthException(message, innerException)
{
}