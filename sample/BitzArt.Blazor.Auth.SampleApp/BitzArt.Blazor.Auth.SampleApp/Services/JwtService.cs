using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BitzArt.Blazor.Auth.SampleApp.Services;

// This service provides a basic example of how to create a JWT.
// In a real-world scenario, you should:
// 1. use a more secure key;
// 2. store it in a secure location;
// and might want to use a more secure algorithm.
public class JwtService
{
    private readonly SigningCredentials _signingCredentials;
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public JwtService()
    {
        var key = "aVeryLongSecretKeyThatIsAtLeast32BytesLong";
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        _signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        _tokenHandler = new JwtSecurityTokenHandler();
    }

    public JwtPair BuildJwtPair()
    {
        var now = DateTime.UtcNow;

        var accessTokenDuration = new TimeSpan(hours: 0, minutes: 15, seconds: 0);
        var accessTokenExpiresAt = now + accessTokenDuration;
        var accessToken = _tokenHandler.WriteToken(new JwtSecurityToken(
            claims:
            [
                new Claim("myClaim", "My claim data")
            ],
            notBefore: now,
            expires: accessTokenExpiresAt,
            signingCredentials: _signingCredentials
        ));

        var refreshTokenDuration = new TimeSpan(hours: 1, minutes: 0, seconds: 0);
        var refreshTokenExpiresAt = now + refreshTokenDuration;
        var refreshToken = _tokenHandler.WriteToken(new JwtSecurityToken(
            notBefore: now,
            expires: refreshTokenExpiresAt,
            signingCredentials: _signingCredentials
        ));

        return new JwtPair(accessToken, accessTokenExpiresAt, refreshToken, refreshTokenExpiresAt);
    }
}