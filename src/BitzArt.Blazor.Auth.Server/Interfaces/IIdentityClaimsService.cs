using System.Security.Claims;

namespace BitzArt.Blazor.Auth.Server;

public interface IIdentityClaimsService
{
    public Task<ClaimsPrincipal> BuildClaimsPrincipalAsync(string accessToken);
}
