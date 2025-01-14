using System.Text.Json.Serialization;

namespace BitzArt.Blazor.Auth;

/// <summary>
/// Represents the result of an authentication operation.
/// </summary>
public class AuthenticationResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the authentication operation was successful.
    /// </summary>
    [JsonPropertyName("isSuccess")]
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets the JWT pair.
    /// </summary>
    [JsonPropertyName("jwtPair")]
    public JwtPair? JwtPair { get; set; }

    /// <summary>
    /// Gets or sets the error message, if any.
    /// </summary>
    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Additional data.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, object> Data { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationResult"/> class.
    /// </summary>
    /// <param name="isSuccess"> Indicates whether the authentication operation was successful. </param>
    /// <param name="jwtPair"> The JWT pair. </param>
    /// <param name="errorMessage"> The error message, if any. </param>
    /// <param name="data"> Additional data. </param>
    public AuthenticationResult(
        bool isSuccess = false,
        JwtPair? jwtPair = null,
        string? errorMessage = null,
        IDictionary<string, object>? data = null)
    {
        IsSuccess = isSuccess;
        JwtPair = jwtPair;
        ErrorMessage = errorMessage;
        Data = data ?? new Dictionary<string, object>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationResult"/> class.
    /// </summary>
    public AuthenticationResult()
    {
        Data = new Dictionary<string, object>();
    }

    /// <summary>
    /// Success authentication result.
    /// </summary>
    /// <param name="jwtPair"> The JWT pair. </param>
    /// <param name="data"> Additional data. </param>
    /// <returns> A new instance of the <see cref="AuthenticationResult"/> class. </returns>
    public static AuthenticationResult Success(JwtPair jwtPair, IDictionary<string, object>? data = null)
        => new(true, jwtPair, data: data);

    /// <summary>
    /// Failure authentication result.
    /// </summary>
    /// <param name="errorMessage"> The error message. </param>
    /// <param name="data"> Additional data. </param>
    /// <returns> A new instance of the <see cref="AuthenticationResult"/> class. </returns>
    public static AuthenticationResult Failure(string errorMessage, IDictionary<string, object>? data = null)
        => new(false, errorMessage: errorMessage, data: data);
}
