using BitzArt.Blazor.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace BitzArt.Blazor.Auth.Server;

// Static server-side implementation of the user service.
internal class StaticUserService(
    IBlazorAuthLogger logger,
    IAuthenticationService authService,
    ICookieService cookieService,
    IIdentityClaimsService claimsService
    ) : IUserService
{
    private protected static AuthenticationState UnauthorizedState => new(new ClaimsPrincipal());

    public async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var cookies = (await cookieService.GetAllAsync())
            .ToDictionary(x => x.Key);

        var accessTokenFound = cookies.TryGetValue(Cookies.AccessToken, out var accessTokenCookie);

        if (accessTokenFound && !string.IsNullOrWhiteSpace(accessTokenCookie!.Value))
        {
            logger.LogDebug("Access token was found in request cookies.");
            var principal = await claimsService.BuildClaimsPrincipalAsync(accessTokenCookie!.Value);
            return new AuthenticationState(principal);
        }

        logger.LogDebug("Access token was not found in request cookies.");

        var refreshTokenFound = cookies.TryGetValue(Cookies.RefreshToken, out var refreshTokenCookie);

        if (refreshTokenFound && !string.IsNullOrWhiteSpace(refreshTokenCookie!.Value))
        {
            logger.LogDebug("Refresh token was found in cookies. Refreshing the user's JWT pair...");

            var refreshResult = await authService.RefreshJwtPairAsync(refreshTokenCookie!.Value);

            if (!refreshResult.IsSuccess)
            {
                logger.LogWarning("Failed to refresh the user's JWT pair.");
                return UnauthorizedState;
            }

            await SaveJwtPair(refreshResult.JwtPair);
            var principal = await claimsService.BuildClaimsPrincipalAsync(refreshResult.JwtPair!.AccessToken!);

            logger.LogDebug("User's JWT pair was successfully refreshed.");

            return new AuthenticationState(principal);
        }

        logger.LogDebug("Refresh token was not found in cookies.");
        return UnauthorizedState;
    }

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
            await cookieService.SetAsync(
                Cookies.AccessToken,
                jwtPair.AccessToken!,
                jwtPair.AccessTokenExpiresAt,
                httpOnly: true,
                secure: true,
                sameSiteMode: SameSiteMode.Strict);

        if (!string.IsNullOrWhiteSpace(jwtPair.RefreshToken))
            await cookieService.SetAsync(
                Cookies.RefreshToken,
                jwtPair.RefreshToken!,
                jwtPair.RefreshTokenExpiresAt,
                httpOnly: true,
                secure: true,
                sameSiteMode: SameSiteMode.Strict);
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
    IBlazorAuthLogger logger,
    IAuthenticationService<TSignInPayload> authService,
    ICookieService cookieService,
    IIdentityClaimsService claimsService
    ) : StaticUserService(logger, authService, cookieService, claimsService), IUserService<TSignInPayload>
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
    IBlazorAuthLogger logger,
    IAuthenticationService<TSignInPayload, TSignUpPayload> authService,
    ICookieService cookieService,
    IIdentityClaimsService claimsService
    ) : StaticUserService<TSignInPayload>(logger, authService, cookieService, claimsService), IUserService<TSignInPayload, TSignUpPayload>
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
