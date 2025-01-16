using BitzArt.Blazor.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.Blazor.Auth.Client;

/// <summary>
/// Extension methods for setting up client-side Blazor.Auth services in a <see cref="WebAssemblyHostBuilder"/>.
/// </summary>
public static class ClientSideAddBlazorAuthExtension
{
    /// <summary>
    /// Adds client-side Blazor.Auth services to the specified <see cref="WebAssemblyHostBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="WebAssemblyHostBuilder"/> to add services to.</param>
    /// <returns><see cref="WebAssemblyHostBuilder"/> to allow chaining.</returns>
    public static WebAssemblyHostBuilder AddBlazorAuth(this WebAssemblyHostBuilder builder)
    {
        builder.AddBlazorCookies();

        builder.AddBlazorAuthHostClient();

        builder.Services.AddAuthorizationCore();
        builder.Services.AddCascadingAuthenticationState();

        builder.Services.AddTransient<ClientSideLogger>();

        builder.Services.AddScoped<ClientSideAuthenticationStateProvider>();
        builder.Services.AddScoped<AuthenticationStateProvider>(x => x.GetRequiredService<ClientSideAuthenticationStateProvider>());

        builder.Services.AddScoped<IUserService, ClientSideUserService>();

        return builder;
    }
}
