using BitzArt.Blazor.Auth;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.AddBlazorAuth();

await builder.Build().RunAsync();
