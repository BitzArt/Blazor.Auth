using System.Security.Claims;

namespace BitzArt.Blazor.Auth;

public interface IIdentityClaimsService
{
    public Task<ClaimsPrincipal> BuildClaimsPrincipalAsync(string accessToken);
}
