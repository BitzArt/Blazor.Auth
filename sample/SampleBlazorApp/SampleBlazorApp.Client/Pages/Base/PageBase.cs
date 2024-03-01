using Microsoft.AspNetCore.Components;

namespace SampleBlazorApp.Client;

public class PageBase : ComponentBase
{
    [Inject]
    public required ILoggerFactory loggerFactory { get; set; }

    private ILogger logger => loggerFactory.CreateLogger("Page");

    protected override void OnInitialized()
    {
        base.OnInitialized();

        logger.LogInformation("Page Initialized");
    }
}
