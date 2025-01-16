namespace BitzArt.Blazor.Auth.Client;

/// <summary>
/// An exception thrown when something goes wrong during the authentication process.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="BlazorAuthException"/> class.
/// </remarks>
/// <param name="message">The error message that explains the reason for the exception.</param>
/// <param name="innerException">The exception that is the cause of the current exception.</param></param>
public class BlazorAuthException(string message, Exception? innerException) : Exception(message, innerException)
{
}
