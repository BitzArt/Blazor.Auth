using System.Text.Json.Serialization;

namespace BitzArt.Blazor.Auth;

public record JwtPair
{
    [JsonPropertyName("accessToken")]
    public string? AccessToken { get; set; }

    [JsonPropertyName("refreshToken")]
    public string? RefreshToken { get; set; }

    [JsonPropertyName("accessTokenExpiresAt")]
    public DateTimeOffset? AccessTokenExpiresAt { get; set; }

    [JsonPropertyName("refreshTokenExpiresAt")]
    public DateTimeOffset? RefreshTokenExpiresAt { get; set; }
}