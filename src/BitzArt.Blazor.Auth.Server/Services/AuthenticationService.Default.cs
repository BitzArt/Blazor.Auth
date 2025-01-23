namespace BitzArt.Blazor.Auth.Server;

internal class DefaultAuthenticationService : AuthenticationService
{
    public override Task<AuthenticationResult> RefreshJwtPairAsync(string refreshToken)
    {
        return Task.FromResult(AuthenticationResult.Failure($"{nameof(IAuthenticationService)}.{nameof(RefreshJwtPairAsync)} is not implemented."));
    }
}
