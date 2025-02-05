using BitzArt.Blazor.Auth.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BitzArt.Blazor.Auth.SampleApp.Client;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.AddBlazorAuth();

        await builder.Build().RunAsync();
    }
}