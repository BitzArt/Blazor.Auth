namespace BitzArt.Blazor.Auth.Server;

internal record AuthenticationServiceSignature(Type? SignInPayloadType, Type? SignUpPayloadType)
{
}