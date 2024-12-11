using System.Security.Claims;

namespace BitzArt.Blazor.Auth;

public interface IIdentityClaimsService
{
    public ClaimsPrincipal BuildClaimsPrincipal(string accessToken);
    public Task<ClaimsPrincipal> BuildClaimsPrincipalAsync(string accessToken);
}
