using BitzArt.Blazor.Auth.Server;

namespace BitzArt.Blazor.Auth.SampleApp.Services;

public class SampleServerSideAuthenticationService(JwtService jwtService)
    : AuthenticationService<SignInPayload, object>()
{
    protected override Task<AuthenticationResult> GetSignInResultAsync(SignInPayload signInPayload)
    {
        var authResult = AuthenticationResult.Success(jwtService.BuildJwtPair());

        return Task.FromResult(authResult);
    }

    public override Task<AuthenticationResult> RefreshJwtPairAsync(string refreshToken)
    {
        var authResult = AuthenticationResult.Success(jwtService.BuildJwtPair());

        return Task.FromResult(authResult);
    }
}