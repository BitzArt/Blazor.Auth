using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BitzArt.Blazor.Auth;

public class IdentityClaimsService() : IIdentityClaimsService
{
    private readonly JwtSecurityTokenHandler _tokenHandler = new();

    public virtual ClaimsPrincipal BuildClaimsPrincipal(string accessToken)
    {
        if (ValidateRawToken(accessToken) == false) return EmptyClaimsPrincipal;

        var token = _tokenHandler.ReadJwtToken(accessToken);

        if (ValidateToken(token) == false) return EmptyClaimsPrincipal;

        var claims = MapClaims(token.Claims);

        claims = claims.Append(new Claim(Constants.AccessTokenName, accessToken));

        return new ClaimsPrincipal(new ClaimsIdentity(claims, "Custom"));
    }

    protected virtual bool ValidateRawToken(string token)
    {
        return true;
    }

    protected virtual bool ValidateToken(JwtSecurityToken token) {
        return true;
    }

    protected virtual IEnumerable<Claim> MapClaims(IEnumerable<Claim> claims) => claims;

    private static ClaimsPrincipal EmptyClaimsPrincipal => new(new ClaimsIdentity());
}
