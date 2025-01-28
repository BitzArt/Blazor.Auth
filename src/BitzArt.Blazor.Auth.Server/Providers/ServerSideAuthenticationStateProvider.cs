using BitzArt.Blazor.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace BitzArt.Blazor.Auth.Server;

internal class ServerSideAuthenticationStateProvider(
    ILoggerFactory loggerFactory,
    ICookieService cookieService,
    ServerSidePrerenderAuthenticationStateProvider prerenderAuth,
    IIdentityClaimsService claimsService,
    IUserService userService)
    : AuthenticationStateProvider
{
    private readonly ILogger _logger = loggerFactory.CreateLogger("Blazor.Auth");
    protected readonly IIdentityClaimsService ClaimsService = claimsService;
    private static AuthenticationState UnauthorizedState => new (new ClaimsPrincipal());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        _logger.LogDebug("GetAuthenticationStateAsync was called.");

        IEnumerable<Cookie>? cookies = null;

        try
        {
            cookies = await cookieService.GetAllAsync();
        }
        catch (Exception)
        {
            _logger.LogDebug("Using IPrerenderAuthenticationStateProvider to retrieve user authentication state.");
            return await prerenderAuth.GetPrerenderAuthenticationStateAsync();
        }

        if (cookies is null) throw new Exception("No cookies array was found.");

        var accessTokenCookie = cookies.FirstOrDefault(c => c.Key == Cookies.AccessToken);
        var refreshTokenCookie = cookies.FirstOrDefault(c => c.Key == Cookies.RefreshToken);

        if (accessTokenCookie is not null && !string.IsNullOrWhiteSpace(accessTokenCookie.Value))
        {
            _logger.LogDebug("Access token was found in cookies.");
            var principal = await ClaimsService.BuildClaimsPrincipalAsync(accessTokenCookie.Value);
            return new AuthenticationState(principal);
        }

        _logger.LogDebug("Access token was not found in cookies.");

        if (refreshTokenCookie is not null && !string.IsNullOrWhiteSpace(refreshTokenCookie.Value))
        {
            _logger.LogDebug("Refresh token was found in cookies. Refreshing the user's JWT pair...");

            var refreshResult = await userService.RefreshJwtPairAsync(refreshTokenCookie.Value);

            if (!refreshResult.IsSuccess)
            {
                _logger.LogWarning("Failed to refresh the user's JWT pair.");
                return UnauthorizedState;
            }

            _logger.LogDebug("User's JWT pair was successfully refreshed.");
            var principal = await ClaimsService.BuildClaimsPrincipalAsync(refreshResult.JwtPair!.AccessToken!);
            return new AuthenticationState(principal);
        }

        _logger.LogDebug("Refresh token was not found in cookies.");
        return UnauthorizedState;
    }
}