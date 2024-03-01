using BitzArt.Blazor.Auth;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddBlazorClientAuth();

builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    });

builder.Logging.AddFilter("Blazor.Auth", LogLevel.Debug);

await builder.Build().RunAsync();
