using BitzArt.Blazor.Auth.Server.Services;
using BitzArt.Blazor.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.Blazor.Auth;

public static class AddBlazorAuthExtension
{
    public static WebAssemblyHostBuilder AddBlazorAuth(this WebAssemblyHostBuilder builder)
    {
        return builder.AddBlazorAuth<ClientSideAuthenticationService>();
    }

    public static WebAssemblyHostBuilder AddBlazorAuth<TAuthenticationService>(this WebAssemblyHostBuilder builder)
        where TAuthenticationService : class, IAuthenticationService
    {
        return builder.AddBlazorAuth<TAuthenticationService, IdentityClaimsService>();
    }

    public static WebAssemblyHostBuilder AddBlazorAuth<TAuthenticationService, TIdentityClaimsService>(this WebAssemblyHostBuilder builder)
        where TAuthenticationService : class, IAuthenticationService
        where TIdentityClaimsService : class, IIdentityClaimsService
    {
        builder.AddBlazorCookies();

        builder.Services.AddAuthorizationCore();
        builder.Services.AddCascadingAuthenticationState();

        builder.Services.AddScoped<IIdentityClaimsService, TIdentityClaimsService>();
        builder.Services.AddScoped<AuthenticationStateProvider, BlazorAuthenticationStateProvider>();
        builder.Services.AddScoped(x => (x.GetRequiredService<AuthenticationStateProvider>() as BlazorAuthenticationStateProvider)!);
        builder.Services.AddSingleton<IPrerenderAuthenticationStateProvider, ClientSidePrerenderAuthenticationStateProvider>();

        // UserService
        builder.Services.AddScoped<IUserService, UserService>();

        // AuthenticationService
        builder.Services.AddHttpClient<IAuthenticationService, TAuthenticationService>(x =>
        {
            x.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress.TrimEnd('/') + "/api");
        });

        return builder;
    }
}
