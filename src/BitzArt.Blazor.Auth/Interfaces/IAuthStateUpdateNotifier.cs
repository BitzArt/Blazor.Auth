namespace BitzArt.Blazor.Auth;

internal interface IAuthStateUpdateNotifier
{
    public delegate void AuthenticationStateUpdatedEventHandler(IUserService? sender, AuthenticationOperationInfo? operationInfo);
    public event AuthenticationStateUpdatedEventHandler? AuthenticationStateUpdated;
}