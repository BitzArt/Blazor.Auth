namespace BitzArt.Blazor.Auth.Server;

public class ServerSideAuthenticationService<TSignInPayload, TSignUpPayload>()
    : IServerSideAuthenticationService
{
    public async Task<AuthenticationResult> SignInAsync(object signInPayload)
    {
        if (signInPayload is not TSignInPayload signInPayloadCasted)
            throw new Exception($"Sign-in payload is not {typeof(TSignInPayload).Name} type.");

        return await SignInAsync(signInPayloadCasted);
    }

    public async Task<AuthenticationResult> SignInAsync(TSignInPayload signInPayload)
    {
        return await GetSignInResultAsync(signInPayload);
    }

    protected virtual Task<AuthenticationResult> GetSignInResultAsync(TSignInPayload signInPayload)
    {
        throw new NotImplementedException();
    }

    public async Task<AuthenticationResult> SignUpAsync(object signUpPayload)
    {
        if (signUpPayload is not TSignUpPayload signUpPayloadCasted)
            throw new Exception($"Sign-up payload is not {typeof(TSignUpPayload).Name} type.");

        return await SignUpAsync(signUpPayloadCasted);
    }

    public async Task<AuthenticationResult> SignUpAsync(TSignUpPayload signUpPayload)
    {
        return await GetSignUpResultAsync(signUpPayload);
    }

    protected virtual Task<AuthenticationResult> GetSignUpResultAsync(TSignUpPayload signIpPayload)
    {
        throw new NotImplementedException();
    }

    public virtual Task<AuthenticationResult> RefreshJwtPairAsync(string refreshToken)
    {
        return Task.FromResult(AuthenticationResult.Failure($"{nameof(IAuthenticationService)}.{nameof(RefreshJwtPairAsync)} is not implemented."));
    }

    public Type? GetSignInPayloadType() => typeof(TSignInPayload);

    public Type? GetSignUpPayloadType() => typeof(TSignUpPayload);
}
