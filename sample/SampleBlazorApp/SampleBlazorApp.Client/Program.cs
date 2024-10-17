using BitzArt.Blazor.Auth;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.AddBlazorAuth();

        await builder.Build().RunAsync();
    }
}