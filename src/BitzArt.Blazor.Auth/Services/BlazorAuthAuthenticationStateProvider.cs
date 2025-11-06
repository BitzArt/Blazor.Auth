using Microsoft.AspNetCore.Components.Authorization;

namespace BitzArt.Blazor.Auth;

/// <inheritdoc/>
internal class BlazorAuthAuthenticationStateProvider : AuthenticationStateProvider, IDisposable
{
    private readonly IUserService _userService;
    private readonly BlazorAuthOptions _options;

#if NET8_0
    private readonly static object _lock = new();
#elif NET9_0_OR_GREATER
    private readonly static Lock _lock = new();
#endif

    public BlazorAuthAuthenticationStateProvider(IUserService userService, BlazorAuthOptions options)
    {
        _userService = userService;

        if (userService is IAuthStateUpdateNotifier notifier)
        {
            notifier.AuthenticationStateUpdated += (sender, info) =>
            {
                _ = GetAuthenticationStateAsync();
            };
        }

        _options = options;
    }

    private Task<AuthenticationState>? _currentTask;
    private Timer? _expirationTimer;
    private AuthenticationState? _cachedState;

    /// <inheritdoc/>
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        lock (_lock)
        {
            // Existing logic for subsequent calls or when option is disabled
            if (_currentTask is not null)
            {
                return _currentTask;
            }

            _currentTask = ResolveStateAsync((authState) =>
            {
                lock (_lock)
                {
                    _currentTask = null;
                    _cachedState = authState;

                    if (!_options.DisableAutoExpirationHandling)
                    {
                        TrySetExpirationTimer(authState);
                    }
                    if (_options.InitialDummyAuthState != null)
                    {
                        // Since it is necessary to notify even if the value is null,
                        // ignore the null check (the reference destination does not use this value)
                        var task = Task.FromResult(authState!);
                        NotifyAuthenticationStateChanged(task);
                    }
                }
            });
            NotifyAuthenticationStateChanged(_currentTask);
        }
        if (_options.InitialDummyAuthState != null)
        {
            var initialResult = _cachedState ?? _options.InitialDummyAuthState;
            return Task.FromResult(initialResult);
        }
        return _currentTask;
    }

    private async Task<AuthenticationState> ResolveStateAsync(Action<AuthenticationState?>? onComplete = null)
    {
        AuthenticationState? result = null;
        try
        {
            result = await _userService.GetAuthenticationStateAsync();
            return result;
        }
        finally
        {
            onComplete?.Invoke(result);
        }
    }

    private void TrySetExpirationTimer(AuthenticationState? authState)
    {
        // Unauthenticated user, no need to set an expiration timer
        if (authState?.User.Identity?.IsAuthenticated != true)
        {
            return;
        }

        // Find access token expiration time, if available
        var expirationClaim = authState.User.Claims.FirstOrDefault(c => c.Type == "exp");

        if (expirationClaim is null || string.IsNullOrWhiteSpace(expirationClaim.Value))
        {
            // no expiration claim found,
            // cannot set an expiration timer
            return;
        }

        if (!long.TryParse(expirationClaim.Value, out var expSeconds))
        {
            // expiration claim value is not a valid number, cannot set an expiration timer
            return;
        }

        var accessExpirationDateTime = DateTimeOffset.FromUnixTimeSeconds(expSeconds);

        // calculate the time until the access token expires
        var timeToExpire = accessExpirationDateTime - DateTimeOffset.UtcNow;

        if (timeToExpire <= TimeSpan.Zero)
        {
            // the token has already expired, refresh the state immediately
            // (however this shouldn't normally be happening)
            _ = Task.Run(() => _userService.RefreshJwtPairAsync());
            return;
        }

        _expirationTimer?.Dispose();

        // set a timer to refresh the user's JWT pair when access token expires
        _expirationTimer = new(_ => Task.Run(() => _userService.RefreshJwtPairAsync()), null, timeToExpire, TimeSpan.Zero);
    }

    // dispose of the refresh timer on component disposal (page close)
    public void Dispose()
    {
        _expirationTimer?.Dispose();
    }
}
