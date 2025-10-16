using Microsoft.AspNetCore.Components.Authorization;

namespace BitzArt.Blazor.Auth;

/// <inheritdoc/>
internal class BlazorAuthAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IUserService _userService;

#if NET8_0
    private readonly static object _lock = new();
#elif NET9_0_OR_GREATER
    private readonly static Lock _lock = new();
#endif

    public BlazorAuthAuthenticationStateProvider(IUserService userService)
    {
        _userService = userService;
    }

    private Task<AuthenticationState>? _currentTask;

    /// <inheritdoc/>
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        lock (_lock)
        {
            if (_currentTask is not null)
            {
                return _currentTask;
            }

            _currentTask = ResolveStateAsync(() =>
            {
                lock (_lock)
                {
                    _currentTask = null;
                }
            });

            NotifyAuthenticationStateChanged(_currentTask);
        }

        return _currentTask;
    }

    private async Task<AuthenticationState> ResolveStateAsync(Action? onComplete = null)
    {
        var result = await _userService.GetAuthenticationStateAsync();

        onComplete?.Invoke();

        return result;
    }
}
