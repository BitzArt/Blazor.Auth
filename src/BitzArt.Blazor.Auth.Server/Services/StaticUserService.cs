using BitzArt.Blazor.Cookies;

namespace BitzArt.Blazor.Auth.Server;

// Static server-side implementation of the user service.
internal class StaticUserService(
    IAuthenticationService authService,
    ICookieService cookieService
    ) : IUserService
{
    public async Task<AuthenticationResultInfo> RefreshJwtPairAsync(string refreshToken)
    {
        var authResult = await authService.RefreshJwtPairAsync(refreshToken) ?? throw new Exception("Authentication result is null.");

        if (authResult?.IsSuccess == true)
            await SaveJwtPair(authResult?.JwtPair);

        return authResult!.GetInfo();
    }

    public async Task SignOutAsync()
    {
        await cookieService.RemoveAsync(Cookies.AccessToken);
        await cookieService.RemoveAsync(Cookies.RefreshToken);
    }

    private protected async Task SaveJwtPair(JwtPair? jwtPair)
    {
        if (jwtPair is null) return;

        if (!string.IsNullOrWhiteSpace(jwtPair.AccessToken))
            await cookieService.SetAsync(Cookies.AccessToken, jwtPair.AccessToken!, jwtPair.AccessTokenExpiresAt);

        if (!string.IsNullOrWhiteSpace(jwtPair.RefreshToken))
            await cookieService.SetAsync(Cookies.RefreshToken, jwtPair.RefreshToken!, jwtPair.RefreshTokenExpiresAt);
    }

    internal static UserServiceRegistrationInfo GetServiceRegistrationInfo(AuthenticationServiceSignature signature)
    {
        if (signature.SignInPayloadType is null)
            return new(GetServiceBasicType());

        if (signature.SignUpPayloadType is null)
            return new(GetServiceSignInType(signature),
                [GetServiceBasicType()]);

        return new(GetServiceSignUpType(signature),
            [GetServiceBasicType(), GetServiceSignInType(signature)]);
    }

    private static Type GetServiceBasicType() => typeof(StaticUserService);
    private static Type GetServiceSignInType(AuthenticationServiceSignature signature) => typeof(StaticUserService<>).MakeGenericType(signature.SignInPayloadType!);
    private static Type GetServiceSignUpType(AuthenticationServiceSignature signature) => typeof(StaticUserService<,>).MakeGenericType(signature.SignInPayloadType!, signature.SignUpPayloadType!);
}

internal class StaticUserService<TSignInPayload>(
    IAuthenticationService<TSignInPayload> authService,
    ICookieService cookieService
    ) : StaticUserService(authService, cookieService), IUserService<TSignInPayload>
{
    private readonly IAuthenticationService<TSignInPayload> authServiceCasted = authService;

    public async Task<AuthenticationResultInfo> SignInAsync(TSignInPayload signInPayload)
    {
        var authResult = await authServiceCasted.SignInAsync(signInPayload) ?? throw new Exception("Authentication result is null.");

        if (authResult.IsSuccess)
            await SaveJwtPair(authResult?.JwtPair);

        return authResult!.GetInfo();
    }
}

internal class StaticUserService<TSignInPayload, TSignUpPayload>(
    IAuthenticationService<TSignInPayload, TSignUpPayload> authService,
    ICookieService cookieService
    ) : StaticUserService<TSignInPayload>(authService, cookieService), IUserService<TSignInPayload, TSignUpPayload>
{
    private readonly IAuthenticationService<TSignInPayload,TSignUpPayload> authServiceCasted = authService;

    public async Task<AuthenticationResultInfo> SignUpAsync(TSignUpPayload signUpPayload)
    {
        var authResult = await authServiceCasted.SignUpAsync(signUpPayload) ?? throw new Exception("Authentication result is null.");

        if (authResult?.IsSuccess == true)
            await SaveJwtPair(authResult?.JwtPair);

        return authResult!.GetInfo();
    }
}
