using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BitzArt.Blazor.Auth.Server;

public class IdentityClaimsService() : IIdentityClaimsService
{
    private readonly JwtSecurityTokenHandler _tokenHandler = new();

    public virtual Task<ClaimsPrincipal> BuildClaimsPrincipalAsync(string accessToken)
    {
        return Task.FromResult(BuildClaimsPrincipal(accessToken));
    }

    public virtual ClaimsPrincipal BuildClaimsPrincipal(string accessToken)
    {
        var token = _tokenHandler.ReadJwtToken(accessToken);

        var claims = MapClaims(token.Claims);
        claims = claims.Append(new Claim(Cookies.AccessToken, accessToken));

        return new ClaimsPrincipal(new ClaimsIdentity(claims, "Custom"));
    }

    protected virtual IEnumerable<Claim> MapClaims(IEnumerable<Claim> claims) => claims;
}
