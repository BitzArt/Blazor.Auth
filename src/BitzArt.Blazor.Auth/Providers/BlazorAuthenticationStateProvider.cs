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
    IUserService userService)
    : AuthenticationStateProvider
{
    private readonly ILogger _logger = loggerFactory.CreateLogger("Blazor.Auth.AuthenticationState");
    protected readonly IIdentityClaimsService ClaimsService = claimsService;
    private static readonly JsonSerializerOptions _logSerializerOptions = new() { WriteIndented = true };
    private static AuthenticationState UnauthorizedState => new(new ClaimsPrincipal());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        _logger.LogDebug("GetAuthenticationStateAsync was called.");

        string? jwtPairJson;

        try
        {
            jwtPairJson = await localStorage.GetItemAsStringAsync(Constants.JwtPairStoragePropertyName);
        }
        catch (Exception)
        {
            _logger.LogDebug("Local storage is not available.");
            return UnauthorizedState;
        }

        JwtPair? jwtPair = null;

        if (jwtPairJson != null)
            jwtPair = JsonSerializer.Deserialize<JwtPair>(jwtPairJson!, BlazorAuthJsonSerializerOptions.Options);

        if (jwtPair is null)
        {
            _logger.LogDebug("JWT pair was not found.");

            return UnauthorizedState;
        }

        if (string.IsNullOrEmpty(jwtPair.AccessToken) || IsExpired(jwtPair.AccessTokenExpiresAt))
        {
            if (string.IsNullOrEmpty(jwtPair.RefreshToken) || IsExpired(jwtPair.RefreshTokenExpiresAt))
            {
                _logger.LogDebug("Access token was not found.");
                return UnauthorizedState;
            }

            var refreshResult = await userService.RefreshJwtPairAsync(jwtPair.RefreshToken);

            if (!refreshResult.IsSuccess)
            {
                _logger.LogDebug("Refresh JWT pair returned {resultType}.{isSuccess}: false\nError Message: {errorMessage}",
                    nameof(AuthenticationResult), nameof(AuthenticationResult.IsSuccess), refreshResult.ErrorMessage);
                return UnauthorizedState;
            }

            if (refreshResult.JwtPair is null)
            {
                _logger.LogDebug("Refresh JWT pair returned {resultType} with {jwtPair}: null.",
                    nameof(AuthenticationResult), nameof(AuthenticationResult.JwtPair));
                return UnauthorizedState;
            }

            jwtPair = refreshResult.JwtPair;

            _logger.LogDebug("JWT pair was successfully refreshed:\n{jwtPair}", JsonSerializer.Serialize(jwtPair, _logSerializerOptions));
        }
        else
        {
            _logger.LogDebug("Access token was found: '{token}'.", jwtPair.AccessToken);
        }

        var principal = ClaimsService.BuildClaimsPrincipal(jwtPair.AccessToken!);

        return new AuthenticationState(principal);
    }

    private static bool IsExpired(DateTimeOffset? timestamp)
    {
        return timestamp == null || DateTimeOffset.UtcNow >= timestamp;
    }
}