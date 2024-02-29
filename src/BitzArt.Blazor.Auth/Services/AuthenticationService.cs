
using Blazored.LocalStorage;
using System.Text.Json;

namespace BitzArt.Blazor.Auth;

public class AuthenticationService(ILocalStorageService localStorage) : IAuthenticationService
{
    public ILocalStorageService LocalStorage = localStorage;

    public virtual async Task<AuthenticationResult?> SignInAsync(object signInPayload)
    {
        var authResult = await GetJwtPairAsync(signInPayload);

        if (authResult?.IsSuccess == true)
            await SetToLocalStorage(Constants.JwtPairStoragePropertyName, authResult?.JwtPair);

        return authResult;
    }

    public virtual async Task<AuthenticationResult?> SignUpAsync(object signUpPayload)
    {
        var authResult = await GetSignUpResultAsync(signUpPayload);

        if (authResult?.IsSuccess == true)
            await SetToLocalStorage(Constants.JwtPairStoragePropertyName, authResult?.JwtPair);

        return authResult;
    }

    public async Task<AuthenticationResult?> RefreshAsync(string refreshToken)
    {
        var authResult = await RefreshJwtPairAsync(refreshToken);

        if (authResult?.IsSuccess == true)
            await SetToLocalStorage(Constants.JwtPairStoragePropertyName, authResult?.JwtPair);

        return authResult;
    }

    private async Task SetToLocalStorage(string key, JwtPair? jwtPair)
    {
        if (jwtPair is not null)
        {
            var jwtPairJson = JsonSerializer.Serialize(jwtPair, BlazorAuthJsonSerializerOptions.GetOptions());
            await LocalStorage.SetItemAsStringAsync(key, jwtPairJson);
        }
    }

    public virtual Task<AuthenticationResult?> GetJwtPairAsync(object signInPayload)
    {
        throw new NotImplementedException();
    }

    public virtual Task<AuthenticationResult?> GetSignUpResultAsync(object signUpPayload)
    {
        throw new NotImplementedException();
    }

    public virtual Task<AuthenticationResult?> RefreshJwtPairAsync(string refreshToken)
    {
        return Task.FromResult((AuthenticationResult?)null);
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

public class AuthenticationService<TSignInPayload, TSignUpPayload>(ILocalStorageService localStorage)
    : AuthenticationService(localStorage)
{
    public async Task<AuthenticationResult?> SignInAsync(TSignInPayload signInPayload)
    {
        return await SignInAsync((object)signInPayload);
    }

    public async Task<AuthenticationResult?> SignUpAsync(TSignUpPayload signUpPayload)
    {
        return await SignUpAsync((object)signUpPayload);
    }

    public virtual Task<AuthenticationResult?> GetJwtPairAsync(TSignInPayload signInPayload)
    {
        throw new NotImplementedException();
    }

    public override async Task<AuthenticationResult?> GetJwtPairAsync(object signInPayload)
    {
        if (signInPayload is not TSignInPayload signInPayloadCasted)
            throw new Exception($"Sign-in payload is not {typeof(TSignInPayload).Name} type");

        return await GetJwtPairAsync(signInPayloadCasted);
    }

    public virtual Task<AuthenticationResult?> GetSignUpResultAsync(TSignUpPayload signIpPayload)
    {
        throw new NotImplementedException();
    }

    public override async Task<AuthenticationResult?> GetSignUpResultAsync(object signUpPayload)
    {
        if (signUpPayload is not TSignUpPayload signUpPayloadCasted)
            throw new Exception($"Sign-up payload is not {typeof(TSignUpPayload).Name} type");

        return await GetSignUpResultAsync(signUpPayloadCasted);
    }

    public override Type? GetSignInPayloadType()
    {
        return typeof(TSignInPayload);
    }

    public override Type? GetSignUpPayloadType()
    {
        return typeof(TSignUpPayload);
    }
}
