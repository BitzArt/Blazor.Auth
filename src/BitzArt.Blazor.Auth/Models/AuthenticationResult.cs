using System.Text.Json.Serialization;

namespace BitzArt.Blazor.Auth;

public class AuthenticationResult
{
    public bool? IsSuccess { get; set; }
    public JwtPair? JwtPair { get; set; }
    public string? ErrorMessage { get; set; }

    [JsonExtensionData]
    public Dictionary<string, object>? Data { get; set; }

    public AuthenticationResult(
        bool? isSuccess = null,
        JwtPair? jwtPair = null,
        string? errorMessage = null,
        Dictionary<string, object>? data = null)
    {
        IsSuccess = isSuccess;
        JwtPair = jwtPair;
        ErrorMessage = errorMessage;
        Data = data;
    }

    public AuthenticationResult()
    {
    }
}
