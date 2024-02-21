using Microsoft.AspNetCore.Components.Authorization;

namespace BitzArt.Blazor.Auth;

public class TestService
{
    public TestService(UserState state, ILogger<TestService> logger)
    {
        logger.LogInformation("Access token {token}", state.AccessToken);   
    }
}
