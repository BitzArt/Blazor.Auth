using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace BitzArt.Blazor.Auth.Server.Services;

internal class ServerSidePrerenderAuthenticationStateProvider(
    IHttpContextAccessor httpContextAccessor,
    IIdentityClaimsService claimsService,
    ILoggerFactory loggerFactory
    ) : IPrerenderAuthenticationStateProvider
{
    private ILogger logger = loggerFactory.CreateLogger("Blazor.Auth.Prerender");
    private static AuthenticationState UnauthorizedState => new(new ClaimsPrincipal());

    public Task<AuthenticationState> GetPrerenderAuthenticationStateAsync()
    {
        var httpContext = httpContextAccessor.HttpContext ?? throw new Exception("HttpContext is not available.");
    
        var cookies = httpContext.Request.Cookies;

        var accessToken = cookies[Constants.AccessTokenCookieName];

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            logger.LogDebug("Access token was not found in request cookies.");
            return Task.FromResult(UnauthorizedState);
        }

        logger.LogDebug("Access token was found in request cookies.");
        var principal = claimsService.BuildClaimsPrincipal(accessToken);
        return Task.FromResult(new AuthenticationState(principal));
    }
}
