using BitzArt.Blazor.Auth;

namespace SampleBlazorApp.Services;

public class JwtService
{
    private static readonly TimeSpan _accessTokenDuration = new(0, 1, 0);
    private static readonly TimeSpan _refreshTokenDuration = new(1, 0, 0);

    public JwtPair BuildJwtPair()
    {
        var now = DateTime.UtcNow;

        var accessToken = "AccessToken";
        var accessTokenExpiresAt = now + _accessTokenDuration;

        var refreshToken = "RefreshToken";
        var refreshTokenExpiresAt = now + _refreshTokenDuration;

        return new JwtPair
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiresAt = accessTokenExpiresAt,
            RefreshTokenExpiresAt = refreshTokenExpiresAt
        };
    }
}