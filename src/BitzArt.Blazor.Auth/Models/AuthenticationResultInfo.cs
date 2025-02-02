using System.Text.Json.Serialization;

namespace BitzArt.Blazor.Auth;

/// <summary>
/// Contains information about the result of an authentication operation.
/// </summary>
public class AuthenticationResultInfo
{
    /// <summary>
    /// Gets or sets a value indicating whether the authentication operation was successful.
    /// </summary>
    [JsonPropertyName("isSuccess")]
    public bool IsSuccess { get; set; }

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
    /// Initializes a new instance of the <see cref="AuthenticationResultInfo"/> class.
    /// </summary>
    /// <param name="isSuccess"> Indicates whether the authentication operation was successful. </param>
    /// <param name="errorMessage"> The error message, if any. </param>
    /// <param name="data"> Additional data. </param>
    public AuthenticationResultInfo(
        bool isSuccess = false,
        string? errorMessage = null,
        IDictionary<string, object>? data = null)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        Data = data ?? new Dictionary<string, object>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationResultInfo"/> class.
    /// </summary>
    public AuthenticationResultInfo()
    {
        Data = new Dictionary<string, object>();
    }
}
