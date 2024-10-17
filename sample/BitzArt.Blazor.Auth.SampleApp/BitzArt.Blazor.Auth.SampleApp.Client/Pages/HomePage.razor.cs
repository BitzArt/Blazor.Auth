using Microsoft.AspNetCore.Components;

namespace BitzArt.Blazor.Auth.SampleApp;

public partial class HomePage
{
    [Inject] NavigationManager NavigationManager { get; set; } = null!;
    [Inject] IUserService UserService { get; set; } = null!;

    private async Task SignInAsync()
    {
        var payload = new SignInPayload("some data");
        var authenticationResult = await UserService.SignInAsync(payload);
        NavigationManager.NavigateTo("/", true);
    }

    private async Task SignOutAsync()
    {
        await UserService.SignOutAsync();
        NavigationManager.NavigateTo("/", true);
    }
}
