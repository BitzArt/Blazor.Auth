namespace BitzArt.Blazor.Auth;

public interface IServerSideAuthenticationService : IAuthenticationService
{
    public Type? GetSignInPayloadType();
    public Type? GetSignUpPayloadType();
}
