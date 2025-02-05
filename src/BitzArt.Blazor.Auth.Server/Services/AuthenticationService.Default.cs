namespace BitzArt.Blazor.Auth.Server;

/// <summary>
/// This class is made internal in order to better comply with the Open/Closed Principle
/// by not allowing the modification of any behavior defined in this class.
/// Any overrides should be done to the base class: <see cref="AuthenticationService"/>.
/// </summary>
internal class DefaultAuthenticationService : AuthenticationService
{
    public override Task<AuthenticationResult> RefreshJwtPairAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(AuthenticationResult.Failure($"{nameof(IAuthenticationService)}.{nameof(RefreshJwtPairAsync)} is not implemented."));
    }
}
