﻿using BitzArt.Blazor.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace BitzArt.Blazor.Auth.Server;

// Static server-side implementation of the user service.
internal class StaticUserService(
    IBlazorAuthLogger logger,
    IAuthenticationService authService,
    ICookieService cookieService,
    IIdentityClaimsService claimsService,
    BlazorAuthServerOptions options
    ) : IUserService
{
    private protected static AuthenticationState UnauthorizedState => new(new ClaimsPrincipal());

    private Cookie? _accessTokenCookie;
    private Cookie? _refreshTokenCookie;

    public async Task<AuthenticationState> GetAuthenticationStateAsync(CancellationToken cancellationToken = default)
    {
        var cookies = (await cookieService.GetAllAsync())
            .ToDictionary(x => x.Key);

        var accessTokenFound = _accessTokenCookie is not null || cookies.TryGetValue(Cookies.AccessToken, out _accessTokenCookie);

        if (accessTokenFound && !string.IsNullOrWhiteSpace(_accessTokenCookie!.Value))
        {
            logger.LogDebug("Access token was found in request cookies.");
            var principal = await claimsService.BuildClaimsPrincipalAsync(_accessTokenCookie!.Value);
            return new AuthenticationState(principal);
        }

        logger.LogDebug("Access token was not found in request cookies.");

        var refreshTokenFound = _refreshTokenCookie is not null || cookies.TryGetValue(Cookies.RefreshToken, out _refreshTokenCookie);

        if (refreshTokenFound && !string.IsNullOrWhiteSpace(_refreshTokenCookie!.Value))
        {
            logger.LogDebug("Refresh token was found in cookies. Refreshing the user's JWT pair...");

            var refreshResult = await authService.RefreshJwtPairAsync(_refreshTokenCookie!.Value, cancellationToken);

            if (!refreshResult.IsSuccess)
            {
                logger.LogWarning("Failed to refresh the user's JWT pair.");
                return UnauthorizedState;
            }

            await SaveJwtPairAsync(refreshResult.JwtPair, cancellationToken);
            var principal = await claimsService.BuildClaimsPrincipalAsync(refreshResult.JwtPair!.AccessToken!);

            logger.LogDebug("User's JWT pair was successfully refreshed.");

            return new AuthenticationState(principal);
        }

        logger.LogDebug("Refresh token was not found in cookies.");
        return UnauthorizedState;
    }

    public async Task<AuthenticationOperationInfo> RefreshJwtPairAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var authResult = await authService.RefreshJwtPairAsync(refreshToken, cancellationToken)
            ?? throw new Exception("Authentication result is null.");

        if (authResult?.IsSuccess == true)
            await SaveJwtPairAsync(authResult?.JwtPair, cancellationToken);

        return authResult!.GetInfo();
    }

    public async Task SignOutAsync(CancellationToken cancellationToken = default)
    {
        await cookieService.RemoveAsync(Cookies.AccessToken, cancellationToken);
        await cookieService.RemoveAsync(Cookies.RefreshToken, cancellationToken);
    }

    private protected async Task SaveJwtPairAsync(JwtPair? jwtPair, CancellationToken cancellationToken = default)
    {
        if (jwtPair is null) return;

        var secure = !options.DisableSecureCookieFlag;

        if (!string.IsNullOrWhiteSpace(jwtPair.AccessToken))
        {
            _accessTokenCookie = new(
                Cookies.AccessToken,
                jwtPair.AccessToken!,
                jwtPair.AccessTokenExpiresAt,
                httpOnly: true,
                secure: secure,
                sameSiteMode: SameSiteMode.Strict);

            await cookieService.SetAsync(_accessTokenCookie, cancellationToken: cancellationToken);
        }
            

        if (!string.IsNullOrWhiteSpace(jwtPair.RefreshToken))
        {
            _refreshTokenCookie = new(
                Cookies.RefreshToken,
                jwtPair.RefreshToken!,
                jwtPair.RefreshTokenExpiresAt,
                httpOnly: true,
                secure: secure,
                sameSiteMode: SameSiteMode.Strict);

            await cookieService.SetAsync(_refreshTokenCookie, cancellationToken: cancellationToken);
        }
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
    IIdentityClaimsService claimsService,
    BlazorAuthServerOptions options
    ) : StaticUserService(logger, authService, cookieService, claimsService, options), IUserService<TSignInPayload>
{
    private readonly IAuthenticationService<TSignInPayload> authServiceCasted = authService;

    public async Task<AuthenticationOperationInfo> SignInAsync(TSignInPayload signInPayload, CancellationToken cancellationToken = default)
    {
        var authResult = await authServiceCasted.SignInAsync(signInPayload, cancellationToken)
            ?? throw new Exception("Authentication result is null.");

        if (authResult.IsSuccess)
            await SaveJwtPairAsync(authResult?.JwtPair, cancellationToken);

        return authResult!.GetInfo();
    }
}

internal class StaticUserService<TSignInPayload, TSignUpPayload>(
    IBlazorAuthLogger logger,
    IAuthenticationService<TSignInPayload, TSignUpPayload> authService,
    ICookieService cookieService,
    IIdentityClaimsService claimsService,
    BlazorAuthServerOptions options
    ) : StaticUserService<TSignInPayload>(logger, authService, cookieService, claimsService, options), IUserService<TSignInPayload, TSignUpPayload>
{
    private readonly IAuthenticationService<TSignInPayload, TSignUpPayload> authServiceCasted = authService;

    public async Task<AuthenticationOperationInfo> SignUpAsync(TSignUpPayload signUpPayload, CancellationToken cancellationToken = default)
    {
        var authResult = await authServiceCasted.SignUpAsync(signUpPayload, cancellationToken)
            ?? throw new Exception("Authentication result is null.");

        if (authResult?.IsSuccess == true)
            await SaveJwtPairAsync(authResult?.JwtPair, cancellationToken);

        return authResult!.GetInfo();
    }
}
