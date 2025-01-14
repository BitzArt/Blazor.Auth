using System.Text.Json.Serialization;

namespace BitzArt.Blazor.Auth;

/// <summary>
/// Represents a pair of JWTs: an access token and a refresh token.
/// </summary>
public record JwtPair
{
    /// <summary>
    /// Gets or sets the access token.
    /// </summary>
    [JsonPropertyName("accessToken")]
    public string? AccessToken { get; set; }

    /// <summary>
    /// Gets or sets the refresh token.
    /// </summary>
    [JsonPropertyName("refreshToken")]
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Gets or sets the expiration date and time of the access token.
    /// </summary>
    [JsonPropertyName("accessTokenExpiresAt")]
    public DateTimeOffset? AccessTokenExpiresAt { get; set; }

    /// <summary>
    /// Gets or sets the expiration date and time of the refresh token.
    /// </summary>
    [JsonPropertyName("refreshTokenExpiresAt")]
    public DateTimeOffset? RefreshTokenExpiresAt { get; set; }
}