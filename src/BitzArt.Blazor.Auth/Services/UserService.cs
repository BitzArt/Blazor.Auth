using Blazored.LocalStorage;
using System.Text.Json;

namespace BitzArt.Blazor.Auth;

internal class UserService(IAuthenticationService auth, ILocalStorageService localStorage) : IUserService
{
    public virtual async Task<AuthenticationResult> SignInAsync(object signInPayload)
    {
        var authResult = await auth.SignInAsync(signInPayload) ?? throw new Exception("Authentication result is null");
        
        if (authResult.IsSuccess)
            await SetToLocalStorage(Constants.JwtPairStoragePropertyName, authResult?.JwtPair);

        return authResult!;
    }

    public virtual async Task<AuthenticationResult> SignUpAsync(object signUpPayload)
    {
        var authResult = await auth.SignUpAsync(signUpPayload) ?? throw new Exception("Authentication result is null");

        if (authResult?.IsSuccess == true)
            await SetToLocalStorage(Constants.JwtPairStoragePropertyName, authResult?.JwtPair);

        return authResult!;
    }

    public async Task<AuthenticationResult> RefreshJwtPairAsync(string refreshToken)
    {
        var authResult = await auth.RefreshJwtPairAsync(refreshToken) ?? throw new Exception("Authentication result is null");

        if (authResult?.IsSuccess == true)
            await SetToLocalStorage(Constants.JwtPairStoragePropertyName, authResult?.JwtPair);

        return authResult!;
    }

    private async Task SetToLocalStorage(string key, JwtPair? jwtPair)
    {
        if (jwtPair is null) return;

        var jwtPairJson = JsonSerializer.Serialize(jwtPair, BlazorAuthJsonSerializerOptions.Options);
        await localStorage.SetItemAsStringAsync(key, jwtPairJson);
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
    ILocalStorageService localStorage
    ) : UserService(auth, localStorage)
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
