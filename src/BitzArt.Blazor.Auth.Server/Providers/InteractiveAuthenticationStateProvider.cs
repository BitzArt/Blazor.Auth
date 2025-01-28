using BitzArt.Blazor.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace BitzArt.Blazor.Auth.Server;

internal class InteractiveAuthenticationStateProvider(
    ICookieService cookieService,
    IIdentityClaimsService claimsService,
    IUserService userService,
    IBlazorAuthLogger logger)
    : BlazorAuthAuthenticationStateProvider
{
    protected readonly IIdentityClaimsService ClaimsService = claimsService;

    private protected override async Task<AuthenticationState> ResolveAuthenticationStateAsync()
    {
        logger.LogDebug("GetAuthenticationStateAsync was called.");

        IEnumerable<Cookie>? cookies = null;

        cookies = await cookieService.GetAllAsync();

        if (cookies is null) throw new Exception("No cookies array was found.");

        var accessTokenCookie = cookies.FirstOrDefault(c => c.Key == Cookies.AccessToken);
        var refreshTokenCookie = cookies.FirstOrDefault(c => c.Key == Cookies.RefreshToken);

        if (accessTokenCookie is not null && !string.IsNullOrWhiteSpace(accessTokenCookie.Value))
        {
            logger.LogDebug("Access token was found in cookies.");
            var principal = await ClaimsService.BuildClaimsPrincipalAsync(accessTokenCookie.Value);
            return new AuthenticationState(principal);
        }

        logger.LogDebug("Access token was not found in cookies.");

        if (refreshTokenCookie is not null && !string.IsNullOrWhiteSpace(refreshTokenCookie.Value))
        {
            logger.LogDebug("Refresh token was found in cookies. Refreshing the user's JWT pair...");

            var refreshResult = await userService.RefreshJwtPairAsync(refreshTokenCookie.Value);

            if (!refreshResult.IsSuccess)
            {
                logger.LogWarning("Failed to refresh the user's JWT pair.");
                return UnauthorizedState;
            }

            logger.LogDebug("User's JWT pair was successfully refreshed.");
            var principal = await ClaimsService.BuildClaimsPrincipalAsync(refreshResult.JwtPair!.AccessToken!);
            return new AuthenticationState(principal);
        }

        logger.LogDebug("Refresh token was not found in cookies.");
        return UnauthorizedState;
    }
}