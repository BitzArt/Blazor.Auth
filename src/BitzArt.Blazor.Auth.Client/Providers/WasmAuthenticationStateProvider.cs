using BitzArt.Blazor.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace BitzArt.Blazor.Auth;

public class WasmAuthenticationStateProvider(
    ILoggerFactory loggerFactory,
    ICookieService cookieService,
    IIdentityClaimsService claimsService,
    IUserService userService)
    : AuthenticationStateProvider
{
    private readonly ILogger _logger = loggerFactory.CreateLogger("Blazor.Auth");
    protected readonly IIdentityClaimsService ClaimsService = claimsService;
    private static AuthenticationState UnauthorizedState => new(new ClaimsPrincipal());

    private AuthenticationState? _authenticationState;

    private AuthenticationState Save(AuthenticationState state)
    {
        _authenticationState = state;
        return state;
    }

    public AuthenticationState? AuthenticationState => _authenticationState;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        _logger.LogDebug("GetAuthenticationStateAsync was called.");

        IEnumerable<Cookie>? cookies = null;

        cookies = await cookieService.GetAllAsync();

        if (cookies is null) throw new Exception("No cookies array was found.");

        var accessTokenCookie = cookies.FirstOrDefault(c => c.Key == Constants.AccessTokenCookieName);
        var refreshTokenCookie = cookies.FirstOrDefault(c => c.Key == Constants.RefreshTokenCookieName);

        if (accessTokenCookie is not null && !string.IsNullOrWhiteSpace(accessTokenCookie.Value))
        {
            _logger.LogDebug("Access token was found in cookies.");
            var principal = await ClaimsService.BuildClaimsPrincipalAsync(accessTokenCookie.Value);
            return Save(new AuthenticationState(principal));
        }

        _logger.LogDebug("Access token was not found in cookies.");

        if (refreshTokenCookie is not null && !string.IsNullOrWhiteSpace(refreshTokenCookie.Value))
        {
            _logger.LogDebug("Refresh token was found in cookies. Refreshing the user's JWT pair...");

            var refreshResult = await userService.RefreshJwtPairAsync(refreshTokenCookie.Value);

            if (!refreshResult.IsSuccess)
            {
                _logger.LogWarning("Failed to refresh the user's JWT pair.");
                return Save(UnauthorizedState);
            }

            _logger.LogDebug("User's JWT pair was successfully refreshed.");
            var principal = await ClaimsService.BuildClaimsPrincipalAsync(refreshResult.JwtPair!.AccessToken!);
            return Save(new AuthenticationState(principal));
        }

        _logger.LogDebug("Refresh token was not found in cookies.");
        return Save(UnauthorizedState);
    }
}