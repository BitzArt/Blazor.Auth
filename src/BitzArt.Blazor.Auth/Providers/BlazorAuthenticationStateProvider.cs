using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text.Json;

namespace BitzArt.Blazor.Auth;

public class BlazorAuthenticationStateProvider(
    ILoggerFactory loggerFactory,
    ILocalStorageService localStorage,
    IIdentityClaimsService claimsService,
    IAuthenticationService authService) 
    : AuthenticationStateProvider
{
    private ILogger _logger = loggerFactory.CreateLogger("Blazor.Auth.AuthenticationState");
    private static JsonSerializerOptions _logSerializerOptions = new()
    {
        WriteIndented = true,
    };

    protected readonly IIdentityClaimsService ClaimsService = claimsService;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        _logger.LogDebug("GetAuthenticationStateAsync was called");

        JwtPair? jwtPair = null;

        try
        {
            var jwtPairJson = await localStorage.GetItemAsStringAsync(Constants.JwtPairStoragePropertyName);
            jwtPair = JsonSerializer.Deserialize<JwtPair>(jwtPairJson!, BlazorAuthJsonSerializerOptions.GetOptions());
        }
        catch (Exception)
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity("PrerenderAuth", "Unauthorized", "Unauthorized")));
        }

        if (jwtPair is null) {
            _logger.LogDebug("JWT pair was not found");

            return new AuthenticationState(new ClaimsPrincipal());
        }

        if (string.IsNullOrEmpty(jwtPair.AccessToken) || IsExpired(jwtPair.AccessTokenExpiresAt))
        {
            if (string.IsNullOrEmpty(jwtPair.RefreshToken) || IsExpired(jwtPair.RefreshTokenExpiresAt))
            {
                _logger.LogDebug("Access token was not found");

                return new AuthenticationState(new ClaimsPrincipal());
            }

            var newJwtPair = await authService.RefreshAsync(jwtPair.RefreshToken);

            if (newJwtPair is null) {
                _logger.LogDebug("Could not refresh JWT pair");

                return new AuthenticationState(new ClaimsPrincipal());
            }

            jwtPair = newJwtPair;

            _logger.LogDebug("JWT pair was successfully refreshed:\n{jwtPair}", JsonSerializer.Serialize(jwtPair, _logSerializerOptions));
        } else
        {
            _logger.LogDebug("Access token was found: '{token}'", jwtPair.AccessToken);
        }

        var principal = ClaimsService.BuildClaimsPrincipal(jwtPair.AccessToken);

        return new AuthenticationState(principal);
    }

    private bool IsExpired(DateTimeOffset? timestamp)
    {
        if (timestamp is null) return true;

        if (DateTimeOffset.UtcNow >= timestamp) return true;

        return false;
    }
}