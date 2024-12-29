using BitzArt.Blazor.Auth.Server.Services;
using BitzArt.Blazor.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BitzArt.Blazor.Auth;

public static class AddBlazorAuthExtension
{
    public static IHostApplicationBuilder AddBlazorAuth(this IHostApplicationBuilder builder)
    {
        return builder.AddBlazorAuth<ServerSideAuthenticationService<object, object>, IdentityClaimsService>();
    }

    public static IHostApplicationBuilder AddBlazorAuth<TServerSideAuthenticationService>(this IHostApplicationBuilder builder)
        where TServerSideAuthenticationService : class, IServerSideAuthenticationService
    {
        return builder.AddBlazorAuth<TServerSideAuthenticationService, IdentityClaimsService>();
    }

    public static IHostApplicationBuilder AddBlazorAuth<TServerSideAuthenticationService, TIdentityClaimsService>(this IHostApplicationBuilder builder)
        where TServerSideAuthenticationService : class, IServerSideAuthenticationService
        where TIdentityClaimsService : class, IIdentityClaimsService
    {
        builder.AddBlazorCookies();

        builder.Services.AddCascadingAuthenticationState();

        builder.Services.AddScoped<IIdentityClaimsService, TIdentityClaimsService>();
        builder.Services.AddScoped<AuthenticationStateProvider, BlazorAuthenticationStateProvider>();
        builder.Services.AddScoped<IPrerenderAuthenticationStateProvider, ServerSidePrerenderAuthenticationStateProvider>();
        builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddlewareResultHandler>();

        builder.Services.ConfigureHttpJsonOptions(opts =>
        {
            opts.SerializerOptions.PropertyNameCaseInsensitive = true;
            opts.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            opts.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

        // UserService
        builder.Services.AddScoped<IUserService, UserService>();

        // AuthenticationService
        builder.Services.AddScoped<IAuthenticationService, TServerSideAuthenticationService>();
        builder.Services.AddScoped(sp => (IServerSideAuthenticationService)sp.GetRequiredService<IAuthenticationService>());

        return builder;
    }
}
