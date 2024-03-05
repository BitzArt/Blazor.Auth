using BitzArt.Blazor.Cookies;

namespace BitzArt.Blazor.Auth;

internal class UserService(
    IAuthenticationService auth,
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
        await cookieService.RemoveAsync(Constants.AccessTokenCookieName);
        await cookieService.RemoveAsync(Constants.RefreshTokenCookieName);
    }

    private async Task SaveJwtPair(JwtPair? jwtPair)
    {
        if (jwtPair is null) return;

        if (!string.IsNullOrWhiteSpace(jwtPair.AccessToken))
            await cookieService.SetAsync(Constants.AccessTokenCookieName, jwtPair.AccessToken!, jwtPair.AccessTokenExpiresAt);

        if (!string.IsNullOrWhiteSpace(jwtPair.RefreshToken))
            await cookieService.SetAsync(Constants.RefreshTokenCookieName, jwtPair.RefreshToken!, jwtPair.RefreshTokenExpiresAt);
    }

    public virtual Type? GetSignInPayloadType()
    {
        return null;
    }

    public virtual Type? GetSignUpPayloadType()
    {
        return null;
    }
}

internal class UserService<TSignInPayload, TSignUpPayload>(
    IAuthenticationService auth,
    ICookieService cookieService
    ) : UserService(auth, cookieService)
{
    public override Type? GetSignInPayloadType()
    {
        return typeof(TSignInPayload);
    }

    public override Type? GetSignUpPayloadType()
    {
        return typeof(TSignUpPayload);
    }
}
