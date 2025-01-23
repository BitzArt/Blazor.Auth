using Microsoft.AspNetCore.Components;

namespace BitzArt.Blazor.Auth.SampleApp;

public class Redirect : ComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    [Parameter, EditorRequired] public required string To { get; set; }

    protected override void OnInitialized()
    {
        NavigationManager.NavigateTo(To);
    }
}
