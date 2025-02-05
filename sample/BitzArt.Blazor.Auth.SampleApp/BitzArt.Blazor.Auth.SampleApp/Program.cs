using BitzArt.Blazor.Auth.SampleApp.Services;
using BitzArt.Blazor.Auth.Server;

namespace BitzArt.Blazor.Auth.SampleApp;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        builder.Services.AddScoped<JwtService>();
        builder.AddBlazorAuth<SampleAuthenticationService>();

        var app = builder.Build();

        app.UseWebAssemblyDebugging();

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(Shared._Imports).Assembly);

        app.MapAuthEndpoints();

        app.Run();
    }
}