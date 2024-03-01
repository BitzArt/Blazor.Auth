using System.Text.Json.Serialization;

namespace BitzArt.Blazor.Auth;

public class AuthenticationResult
{
    [JsonPropertyName("isSuccess")]
    public bool IsSuccess { get; set; }

    [JsonPropertyName("jwtPair")]
    public JwtPair? JwtPair { get; set; }

    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> Data { get; set; }

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

    public AuthenticationResult()
    {
        Data = new Dictionary<string, object>();
    }

    public static AuthenticationResult Success(JwtPair jwtPair, IDictionary<string, object>? data = null)
        => new(true, jwtPair, data: data);

    public static AuthenticationResult Failure(string errorMessage, IDictionary<string, object>? data = null)
        => new(false, errorMessage: errorMessage, data: data);
}
