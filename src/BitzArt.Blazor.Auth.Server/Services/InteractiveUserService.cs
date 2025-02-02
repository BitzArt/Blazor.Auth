namespace BitzArt.Blazor.Auth.Server;

// Interactive server-side implementation of the user service.
internal class InteractiveUserService() : IUserService
{
    public Task<AuthenticationResultInfo> RefreshJwtPairAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public Task SignOutAsync()
    {
        throw new NotImplementedException();
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

    private static Type GetServiceBasicType() => typeof(InteractiveUserService);
    private static Type GetServiceSignInType(AuthenticationServiceSignature signature) => typeof(InteractiveUserService<>).MakeGenericType(signature.SignInPayloadType!);
    private static Type GetServiceSignUpType(AuthenticationServiceSignature signature) => typeof(InteractiveUserService<,>).MakeGenericType(signature.SignInPayloadType!, signature.SignUpPayloadType!);
}

internal class InteractiveUserService<TSignInPayload> : InteractiveUserService, IUserService<TSignInPayload>
{
    public Task<AuthenticationResultInfo> SignInAsync(TSignInPayload signInPayload)
    {
        throw new NotImplementedException();
    }
}

internal class InteractiveUserService<TSignInPayload, TSignUpPayload> : InteractiveUserService<TSignInPayload>, IUserService<TSignInPayload, TSignUpPayload>
{
    public virtual Task<AuthenticationResultInfo> SignUpAsync(TSignUpPayload signUpPayload)
    {
        throw new NotImplementedException();
    }
}
