using Microsoft.AspNetCore.Components.Authorization;

namespace BitzArt.Blazor.Auth;

/// <inheritdoc/>
internal class BlazorAuthAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IUserService _userService;

    private readonly SemaphoreSlim _semaphore;
    private Task<AuthenticationState>? _authenticationStateTask;

    public BlazorAuthAuthenticationStateProvider(IUserService userService)
    {
        _userService = userService;

        _semaphore = new(1);
    }

    /// <inheritdoc/>
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        bool isPrimary = false;
        Task<AuthenticationState>? task = null;

        await _semaphore.WaitAsync();

        try
        {
            if (_authenticationStateTask is null)
            {
                isPrimary = true;
                task = _userService.GetAuthenticationStateAsync();
                _authenticationStateTask = task;
            }
            else
            {
                task = _authenticationStateTask;
            } 
        }
        finally
        {
            _semaphore.Release();
        }

        if (isPrimary)
        {
            NotifyAuthenticationStateChanged(_authenticationStateTask);
            var result = await task;
            _authenticationStateTask = null;
            return result;
        }

        return await task!;
    }
}
