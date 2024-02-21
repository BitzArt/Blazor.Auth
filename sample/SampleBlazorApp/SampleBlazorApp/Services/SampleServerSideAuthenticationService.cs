
using Blazored.LocalStorage;
using SampleBlazorApp;
using SampleBlazorApp.Services;

namespace BitzArt.Blazor.Auth;

public class SampleServerSideAuthenticationService(ILocalStorageService localStorage, JwtService jwt) 
    : AuthenticationService<SignInPayload>(localStorage)
{
    public override Task<JwtPair?> GetJwtPairAsync(SignInPayload signInPayload)
    {
        var jwtPair = jwt.BuildJwtPair();
        return Task.FromResult<JwtPair?>(jwtPair);
    }

    public override Task<JwtPair?> RefreshJwtPairAsync(string refreshToken)
    {
        var jwtPair = jwt.BuildJwtPair();
        return Task.FromResult<JwtPair?>(jwtPair);
    }
}
