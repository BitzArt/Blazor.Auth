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
    protected readonly IIdentityClaimsService ClaimsService = claimsService;
    private static JsonSerializerOptions _logSerializerOptions = new() { WriteIndented = true };
    private static AuthenticationState UnauthorizedState => new(new ClaimsPrincipal());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        _logger.LogDebug("GetAuthenticationStateAsync was called");

        string? jwtPairJson;

        try
        {
            jwtPairJson = await localStorage.GetItemAsStringAsync(Constants.JwtPairStoragePropertyName);
        }
        catch (Exception)
        {
            _logger.LogDebug("Local storage is not available");

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity("PrerenderAuth", "Unauthorized", "Unauthorized")));
        }

        JwtPair? jwtPair = null;

        if (jwtPairJson != null)
            jwtPair = JsonSerializer.Deserialize<JwtPair>(jwtPairJson!, BlazorAuthJsonSerializerOptions.GetOptions());

        if (jwtPair is null) {
            _logger.LogDebug("JWT pair was not found");

            return UnauthorizedState;
        }

        if (string.IsNullOrEmpty(jwtPair.AccessToken) || IsExpired(jwtPair.AccessTokenExpiresAt))
        {
            if (string.IsNullOrEmpty(jwtPair.RefreshToken) || IsExpired(jwtPair.RefreshTokenExpiresAt))
            {
                _logger.LogDebug("Access token was not found");
                return UnauthorizedState;
            }

            var refreshResult = await authService.RefreshJetPairAsync(jwtPair.RefreshToken);

            if (refreshResult?.IsSuccess != true)
            {
                _logger.LogDebug("Could not refresh JWT pair");
                return UnauthorizedState;
            }

            if (refreshResult.JwtPair is null)
            {
                _logger.LogDebug("Could not refresh JWT pair");
                return UnauthorizedState;
            }

            jwtPair = refreshResult.JwtPair;

            _logger.LogDebug("JWT pair was successfully refreshed:\n{jwtPair}", JsonSerializer.Serialize(jwtPair, _logSerializerOptions));
        } else
        {
            _logger.LogDebug("Access token was found: '{token}'", jwtPair.AccessToken);
        }

        var principal = ClaimsService.BuildClaimsPrincipal(jwtPair.AccessToken!);

        return new AuthenticationState(principal);
    }

    private static bool IsExpired(DateTimeOffset? timestamp)
    {
        return timestamp == null || DateTimeOffset.UtcNow >= timestamp;
    }
}