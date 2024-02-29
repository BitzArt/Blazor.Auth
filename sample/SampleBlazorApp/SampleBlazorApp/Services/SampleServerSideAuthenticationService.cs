﻿
using Blazored.LocalStorage;
using SampleBlazorApp;
using SampleBlazorApp.Services;

namespace BitzArt.Blazor.Auth;

public class SampleServerSideAuthenticationService(ILocalStorageService localStorage, JwtService jwt)
    : AuthenticationService<SignInPayload, SignUpPayload>(localStorage)
{
    public override Task<AuthenticationResult?> GetSignInResultAsync(SignInPayload signInPayload)
    {
        var authResult = new AuthenticationResult
        {
            IsSuccess = true,
            JwtPair = jwt.BuildJwtPair()
        };

        return Task.FromResult<AuthenticationResult?>(authResult);
    }

    public override Task<AuthenticationResult?> GetRefreshJwtPairResultAsync(string refreshToken)
    {
        var authResult = new AuthenticationResult
        {
            IsSuccess = true,
            JwtPair = jwt.BuildJwtPair()
    };

        return Task.FromResult<AuthenticationResult?>(authResult);
    }
}