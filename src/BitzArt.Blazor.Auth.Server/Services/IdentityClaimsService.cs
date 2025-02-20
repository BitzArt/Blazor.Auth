using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BitzArt.Blazor.Auth.Server;

/// <inheritdoc cref="IIdentityClaimsService"/>
public class IdentityClaimsService : IIdentityClaimsService
{
    private readonly JwtSecurityTokenHandler _tokenHandler = new();

    /// <inheritdoc/>
    public Task<ClaimsPrincipal> BuildClaimsPrincipalAsync(string accessToken)
    {
        return Task.FromResult(BuildClaimsPrincipal(accessToken));
    }

    /// <summary>
    /// Builds a <see cref="ClaimsPrincipal"/> from the provided access token.
    /// </summary>
    /// <param name="accessToken"> The access token to build <see cref="ClaimsPrincipal"/> from. </param>
    /// <returns> A <see cref="ClaimsPrincipal"/> representing the user. </returns>
    public ClaimsPrincipal BuildClaimsPrincipal(string accessToken)
    {
        var token = _tokenHandler.ReadJwtToken(accessToken);

        var claims = MapClaims(token.Claims);
        claims = claims.Append(new Claim(Cookies.AccessToken, accessToken));

        return new ClaimsPrincipal(new ClaimsIdentity(claims, "Custom"));
    }

    /// <summary>
    /// This method can be overridden to map the claims from the token to the <see cref="ClaimsPrincipal"/>.
    /// </summary>
    /// <param name="claims">Claims from the token.</param>
    /// <returns>A collection of claims to be added to the <see cref="ClaimsPrincipal"/>.</returns>
    protected virtual IEnumerable<Claim> MapClaims(IEnumerable<Claim> claims) => claims;
}
