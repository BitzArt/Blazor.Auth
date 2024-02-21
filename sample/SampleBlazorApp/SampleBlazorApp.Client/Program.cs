using BitzArt.Blazor.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SampleBlazorApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<JwtService>();
builder.Services.AddBlazorClientAuth();

builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    });

builder.Services.AddScoped<UserState>();
builder.Services.AddScoped<TestService>();

builder.Logging.AddFilter("Blazor.Auth", LogLevel.Debug);

await builder.Build().RunAsync();
