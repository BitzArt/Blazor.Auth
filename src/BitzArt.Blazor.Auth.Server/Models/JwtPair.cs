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
    public string? AccessToken { get; set; }

    /// <summary>
    /// Gets or sets the expiration date and time of the access token.
    /// </summary>
    public DateTimeOffset? AccessTokenExpiresAt { get; set; }

    /// <summary>
    /// Gets or sets the refresh token.
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Gets or sets the expiration date and time of the refresh token.
    /// </summary>
    public DateTimeOffset? RefreshTokenExpiresAt { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtPair"/> class.
    /// </summary>
    /// <param name="accessToken"> The access token. </param>
    /// <param name="accessTokenExpiresAt"> The expiration date and time of the access token. </param>
    /// <param name="refreshToken"> The refresh token. </param>
    /// <param name="refreshTokenExpiresAt"> The expiration date and time of the refresh token. </param>
    public JwtPair(string accessToken, DateTimeOffset? accessTokenExpiresAt, string? refreshToken = null, DateTimeOffset? refreshTokenExpiresAt = null)
    {
        AccessToken = accessToken;
        AccessTokenExpiresAt = accessTokenExpiresAt;

        RefreshToken = refreshToken;
        RefreshTokenExpiresAt = refreshTokenExpiresAt;
    }
}