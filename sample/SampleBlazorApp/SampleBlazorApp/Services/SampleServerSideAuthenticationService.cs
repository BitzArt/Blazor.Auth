using SampleBlazorApp;
using SampleBlazorApp.Services;

namespace BitzArt.Blazor.Auth;

public class SampleServerSideAuthenticationService(JwtService jwtService)
    : ServerSideAuthenticationService<SignInPayload, SignUpPayload>()
{
    protected override Task<AuthenticationResult> GetSignInResultAsync(SignInPayload signInPayload)
    {
        var authResult = AuthenticationResult.Success(jwtService.BuildJwtPair());

        return Task.FromResult(authResult);
    }

    protected override Task<AuthenticationResult> GetSignUpResultAsync(SignUpPayload signUpPayload)
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