using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.Blazor.Auth.Client;

internal static class AddBlazorAuthHostClientExtension
{
    internal static WebAssemblyHostBuilder AddBlazorAuthHostClient(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddHttpClient<BlazorHostClient>(httpClient =>
        {
            httpClient.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress.TrimEnd('/') + "/_auth");

        }).AddHttpMessageHandler<BlazorHostClientMessageHandler>();

        return builder;
    }
}
