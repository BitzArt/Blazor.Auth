namespace BitzArt.Blazor.Auth;

public class AuthenticationService() : IAuthenticationService
{
    public virtual Task<AuthenticationResult> SignInAsync(object signInPayload)
    {
        throw new NotImplementedException($"Implement {nameof(IAuthenticationService)}.{nameof(SignInAsync)} in order to be able to use the sign-in functionality.");
    }

    public virtual Task<AuthenticationResult> SignUpAsync(object signUpPayload)
    {
        throw new NotImplementedException($"Implement {nameof(IAuthenticationService)}.{nameof(SignUpAsync)} in order to be able to use the sign-up functionality.");
    }

    public virtual Task<AuthenticationResult> RefreshJwtPairAsync(string refreshToken)
    {
        return Task.FromResult(AuthenticationResult.Failure($"{nameof(IAuthenticationService)}.{nameof(RefreshJwtPairAsync)} is not implemented."));
    }
}
