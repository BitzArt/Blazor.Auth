using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.Blazor.Auth.Client;

internal static class AddBlazorAuthHostClientExtension
{
    internal static WebAssemblyHostBuilder AddBlazorAuthHostClient(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped<BlazorHostHttpClientMessageHandler>();

        builder.Services.AddHttpClient<BlazorHostHttpClient>(httpClient =>
        {
            httpClient.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);

        }).AddHttpMessageHandler<BlazorHostHttpClientMessageHandler>();

        return builder;
    }
}
