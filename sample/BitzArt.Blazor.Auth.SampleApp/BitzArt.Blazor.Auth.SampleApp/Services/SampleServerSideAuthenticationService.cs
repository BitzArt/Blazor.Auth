using BitzArt.Blazor.Auth.Server;

namespace BitzArt.Blazor.Auth.SampleApp.Services;

public class SampleServerSideAuthenticationService(JwtService jwtService)
    : AuthenticationService<SignInPayload>()
{
    public override Task<AuthenticationResult> SignInAsync(SignInPayload signInPayload)
    {
        var jwtPair = jwtService.BuildJwtPair();
        var authResult = AuthenticationResult.Success(jwtPair);

        return Task.FromResult(authResult);
    }

    public override Task<AuthenticationResult> RefreshJwtPairAsync(string refreshToken)
    {
        var jwtPair = jwtService.BuildJwtPair();
        var authResult = AuthenticationResult.Success(jwtPair);

        return Task.FromResult(authResult);
    }
}