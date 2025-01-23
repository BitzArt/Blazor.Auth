namespace BitzArt.Blazor.Auth.Server;

// Interactive server-side implementation of the user service.
internal class InteractiveUserService() : IUserService
{
    public Task<AuthenticationResult> RefreshJwtPairAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public Task SignOutAsync()
    {
        throw new NotImplementedException();
    }

    internal static UserServiceRegistrationInfo GetRegistrationInfo(AuthenticationServiceSignature signature)
    {
        if (signature.SignInPayloadType is null)
            return new(GetBasicServiceType());

        if (signature.SignUpPayloadType is null)
            return new(GetSignInServiceType(signature),
                [GetBasicServiceType()]);

        return new(GetSignUpServiceType(signature),
            [GetBasicServiceType(), GetSignInServiceType(signature)]);
    }

    private static Type GetBasicServiceType() => typeof(InteractiveUserService);
    private static Type GetSignInServiceType(AuthenticationServiceSignature signature) => typeof(InteractiveUserService<>).MakeGenericType(signature.SignInPayloadType!);
    private static Type GetSignUpServiceType(AuthenticationServiceSignature signature) => typeof(InteractiveUserService<,>).MakeGenericType(signature.SignInPayloadType!, signature.SignUpPayloadType!);
}

internal class InteractiveUserService<TSignInPayload> : InteractiveUserService, IUserService<TSignInPayload>
{
    public Task<AuthenticationResult> SignInAsync(TSignInPayload signInPayload)
    {
        throw new NotImplementedException();
    }
}

internal class InteractiveUserService<TSignInPayload, TSignUpPayload> : InteractiveUserService<TSignInPayload>, IUserService<TSignInPayload, TSignUpPayload>
{
    public virtual Task<AuthenticationResult> SignUpAsync(TSignUpPayload signUpPayload)
    {
        throw new NotImplementedException();
    }
}
