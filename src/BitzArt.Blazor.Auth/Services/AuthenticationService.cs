
using Blazored.LocalStorage;
using System.Text.Json;

namespace BitzArt.Blazor.Auth;

public class AuthenticationService(ILocalStorageService localStorage) : IAuthenticationService
{
    public ILocalStorageService LocalStorage = localStorage;

    public virtual async Task<JwtPair?> SignInAsync(object signInPayload)
    {
        var jwtPair = await GetJwtPairAsync(signInPayload);

        if (jwtPair == null) return null;

        var jwtPairJson = JsonSerializer.Serialize(jwtPair, BlazorAuthJsonSerializerOptions.GetOptions());
        await LocalStorage.SetItemAsStringAsync(Constants.JwtPairStoragePropertyName, jwtPairJson);

        return jwtPair;
    }

    public async Task<JwtPair?> RefreshAsync(string refreshToken)
    {
        var jwtPair = await RefreshJwtPairAsync(refreshToken);

        if (jwtPair is null) return null;

        var jwtPairJson = JsonSerializer.Serialize(jwtPair, BlazorAuthJsonSerializerOptions.GetOptions());
        await LocalStorage.SetItemAsStringAsync(Constants.JwtPairStoragePropertyName, jwtPairJson);

        return jwtPair;
    }

    public virtual Task<JwtPair?> RefreshJwtPairAsync(string refreshToken)
    {
        return Task.FromResult((JwtPair?)null);
    }

    public virtual Task<JwtPair?> GetJwtPairAsync(object signInPayload)
    {
        throw new NotImplementedException();
    }

    public virtual Type? GetSignInPayloadType()
    {
        return null;
    }
}

public class AuthenticationService<TSignInPayload>(ILocalStorageService localStorage) 
    : AuthenticationService(localStorage)
{
    public async Task<JwtPair?> SignInAsync(TSignInPayload signInPayload)
    {
        return await SignInAsync((object)signInPayload);
    }

    public virtual Task<JwtPair?> GetJwtPairAsync(TSignInPayload signInPayload)
    {
        throw new NotImplementedException();
    }

    public override async Task<JwtPair?> GetJwtPairAsync(object signInPayload)
    {
        if(signInPayload is not TSignInPayload signInPayloadCasted)
            throw new Exception($"Sign in payload is not {typeof(TSignInPayload).Name}.");

        return await GetJwtPairAsync(signInPayloadCasted);
    }

    public override Type? GetSignInPayloadType()
    {
        return typeof(TSignInPayload);
    }
}