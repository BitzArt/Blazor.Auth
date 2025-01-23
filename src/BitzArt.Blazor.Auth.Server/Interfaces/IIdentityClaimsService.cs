using System.Security.Claims;

namespace BitzArt.Blazor.Auth.Server;

/// <summary>
/// This service is responsible for building a <see cref="ClaimsPrincipal"/> based on the provided access token.
/// </summary>
public interface IIdentityClaimsService
{
    /// <summary>
    /// Builds a <see cref="ClaimsPrincipal"/> from the provided access token.
    /// </summary>
    /// <param name="accessToken"> The access token to build <see cref="ClaimsPrincipal"/> from. </param>
    /// <returns> A <see cref="Task"/> representing the asynchronous operation. </returns>
    public Task<ClaimsPrincipal> BuildClaimsPrincipalAsync(string accessToken);
}
