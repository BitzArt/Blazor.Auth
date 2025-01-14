using BitzArt.Blazor.Cookies;

namespace BitzArt.Blazor.Auth.Server;

internal class ServerSideUserService(
    IServerSideAuthenticationService auth,
    ICookieService cookieService
    ) : IUserService
{
    public virtual async Task<AuthenticationResult> SignInAsync(object signInPayload)
    {
        var authResult = await auth.SignInAsync(signInPayload) ?? throw new Exception("Authentication result is null.");

        if (authResult.IsSuccess)
            await SaveJwtPair(authResult?.JwtPair);

        return authResult!;
    }

    public virtual async Task<AuthenticationResult> SignUpAsync(object signUpPayload)
    {
        var authResult = await auth.SignUpAsync(signUpPayload) ?? throw new Exception("Authentication result is null.");

        if (authResult?.IsSuccess == true)
            await SaveJwtPair(authResult?.JwtPair);

        return authResult!;
    }

    public async Task<AuthenticationResult> RefreshJwtPairAsync(string refreshToken)
    {
        var authResult = await auth.RefreshJwtPairAsync(refreshToken) ?? throw new Exception("Authentication result is null.");

        if (authResult?.IsSuccess == true)
            await SaveJwtPair(authResult?.JwtPair);

        return authResult!;
    }

    public async Task SignOutAsync()
    {
        await cookieService.RemoveAsync(Cookies.AccessToken);
        await cookieService.RemoveAsync(Cookies.RefreshToken);
    }

    private async Task SaveJwtPair(JwtPair? jwtPair)
    {
        if (jwtPair is null) return;

        if (!string.IsNullOrWhiteSpace(jwtPair.AccessToken))
            await cookieService.SetAsync(Cookies.AccessToken, jwtPair.AccessToken!, jwtPair.AccessTokenExpiresAt);

        if (!string.IsNullOrWhiteSpace(jwtPair.RefreshToken))
            await cookieService.SetAsync(Cookies.RefreshToken, jwtPair.RefreshToken!, jwtPair.RefreshTokenExpiresAt);
    }
}
