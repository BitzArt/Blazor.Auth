namespace BitzArt.Blazor.Auth;

public class AuthenticationResult
{
    public bool? IsSuccess { get; set; }
    public JwtPair? JwtPair { get; set; }
    public string? ErrorMessage { get; set; }
}
