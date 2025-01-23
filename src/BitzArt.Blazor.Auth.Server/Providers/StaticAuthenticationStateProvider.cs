using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace BitzArt.Blazor.Auth.Server;

internal class StaticAuthenticationStateProvider(
    IHttpContextAccessor httpContextAccessor,
    IIdentityClaimsService claimsService,
    IAuthenticationService authenticationService,
    ILoggerFactory loggerFactory) : AuthenticationStateProvider
{
    private readonly ILogger logger = loggerFactory.CreateLogger("Blazor.Auth");
    private static AuthenticationState UnauthorizedState => new(new ClaimsPrincipal());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var httpContext = httpContextAccessor.HttpContext ?? throw new Exception("HttpContext is not available.");
    
        var cookies = httpContext.Request.Cookies;

        var accessToken = cookies[Cookies.AccessToken];

        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            logger.LogDebug("Access token was found in request cookies.");
            var principal = await claimsService.BuildClaimsPrincipalAsync(accessToken);
            return new AuthenticationState(principal);
        }

        logger.LogDebug("Access token was not found in request cookies.");

        var refreshToken = cookies[Cookies.RefreshToken];

        if (!string.IsNullOrWhiteSpace(refreshToken))
        {
            logger.LogDebug("Refresh token was found in cookies. Refreshing the user's JWT pair...");

            var refreshResult = await authenticationService.RefreshJwtPairAsync(refreshToken);

            if (!refreshResult.IsSuccess)
            {
                logger.LogWarning("Failed to refresh the user's JWT pair.");
                return UnauthorizedState;
            }

            logger.LogDebug("User's JWT pair was successfully refreshed.");
            var principal = await claimsService.BuildClaimsPrincipalAsync(refreshResult.JwtPair!.AccessToken!);
            
            httpContext.Response.Cookies.Append(Cookies.AccessToken, refreshResult.JwtPair!.AccessToken!, new CookieOptions
            {
                SameSite = SameSiteMode.Strict,
                Expires = refreshResult.JwtPair.AccessTokenExpiresAt
            });

            httpContext.Response.Cookies.Append(Cookies.RefreshToken, refreshResult.JwtPair!.RefreshToken!, new CookieOptions
            {
                SameSite = SameSiteMode.Strict,
                Expires = refreshResult.JwtPair.RefreshTokenExpiresAt
            });

            return new AuthenticationState(principal);
        }

        logger.LogDebug("Refresh token was not found in cookies.");
        return UnauthorizedState;
    }
}
