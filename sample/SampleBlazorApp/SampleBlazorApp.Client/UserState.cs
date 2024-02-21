using Microsoft.AspNetCore.Components.Authorization;

namespace BitzArt.Blazor.Auth;

public class UserState
{
    public string? AccessToken { get; set; }

    public UserState(AuthenticationStateProvider stateProvider)
    {
        var state = stateProvider.GetAuthenticationStateAsync().Result;
        AccessToken = state.User.Claims.FirstOrDefault(x => x.Type == "AccessToken")?.Value;
    }
}
