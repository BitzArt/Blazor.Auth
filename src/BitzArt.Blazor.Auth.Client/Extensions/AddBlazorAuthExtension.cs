using BitzArt.Blazor.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.Blazor.Auth.Client;

public static class AddBlazorAuthExtension
{
    public static WebAssemblyHostBuilder AddBlazorAuth(this WebAssemblyHostBuilder builder)
    {
        builder.AddBlazorCookies();

        builder.Services.AddAuthorizationCore();
        builder.Services.AddCascadingAuthenticationState();

        builder.Services.AddScoped<WasmAuthenticationStateProvider>();
        builder.Services.AddScoped<AuthenticationStateProvider>(x => x.GetRequiredService<WasmAuthenticationStateProvider>());

        // UserService
        builder.Services.AddHttpClient<IUserService, ClientSideUserService>(x =>
        {
            x.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress.TrimEnd('/') + "/_auth");
        });

        return builder;
    }
}
