using BitzArt.Blazor.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BitzArt.Blazor.Auth.Server;

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
        builder.Services.AddScoped<AuthenticationStateProvider, ServerSideAuthenticationStateProvider>();
        builder.Services.AddScoped<ServerSidePrerenderAuthenticationStateProvider>();

        // Fix for issue: https://github.com/dotnet/aspnetcore/issues/52317
        builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddlewareResultHandler>();

        // UserService
        builder.Services.AddScoped<IUserService, ServerSideUserService>();

        // AuthenticationService
        builder.Services.AddScoped<IServerSideAuthenticationService, TServerSideAuthenticationService>();

        return builder;
    }
}
