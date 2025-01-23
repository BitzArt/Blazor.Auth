using BitzArt.Blazor.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BitzArt.Blazor.Auth.Server;

/// <summary>
/// Contains extension methods for setting up server-side Blazor.Auth services in a <see cref="IHostApplicationBuilder"/>.
/// </summary>
public static class ServerSideAddBlazorAuthExtensions
{
    /// <summary>
    /// Adds server-side Blazor.Auth services to the specified <see cref="IHostApplicationBuilder"/>, <br />
    /// using default implementations of <see cref="IAuthenticationService"/> and <see cref="IIdentityClaimsService"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IHostApplicationBuilder"/> to add services to.</param>
    /// <returns><see cref="IHostApplicationBuilder"/> to allow chaining.</returns>
    public static IHostApplicationBuilder AddBlazorAuth(this IHostApplicationBuilder builder)
    {
        return builder.AddBlazorAuth<DefaultAuthenticationService, IdentityClaimsService>();
    }

    /// <summary>
    /// Adds server-side Blazor.Auth services to the specified <see cref="IHostApplicationBuilder"/>, <br />
    /// using the default implementation of <see cref="IIdentityClaimsService"/>.
    /// </summary>
    /// <typeparam name="TAuthenticationService">The type of the server-side authentication service.</typeparam>
    public static IHostApplicationBuilder AddBlazorAuth<TAuthenticationService>(this IHostApplicationBuilder builder)
        where TAuthenticationService : class, IAuthenticationService
    {
        return builder.AddBlazorAuth<TAuthenticationService, IdentityClaimsService>();
    }

    /// <summary>
    /// Adds server-side Blazor.Auth services to the specified <see cref="IHostApplicationBuilder"/>.
    /// </summary>
    /// <typeparam name="TAuthenticationService">The type of the server-side authentication service.</typeparam>
    /// <typeparam name="TIdentityClaimsService">The type of the identity claims service.</typeparam>
    public static IHostApplicationBuilder AddBlazorAuth<TAuthenticationService, TIdentityClaimsService>(this IHostApplicationBuilder builder)
        where TAuthenticationService : class, IAuthenticationService
        where TIdentityClaimsService : class, IIdentityClaimsService
    {
        builder.AddBlazorCookies();

        builder.Services.AddCascadingAuthenticationState();

        builder.Services.AddScoped<IIdentityClaimsService, TIdentityClaimsService>();
        builder.Services.AddScoped<AuthenticationStateProvider, ServerSideAuthenticationStateProvider>();
        builder.Services.AddScoped<ServerSidePrerenderAuthenticationStateProvider>();

        builder.AddAuthenticationService<TAuthenticationService>();

        // Fix for issue: https://github.com/dotnet/aspnetcore/issues/52317
        builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddlewareResultHandler>();

        // UserService
        builder.Services.AddScoped<IUserService, ServerSideUserService>();

        return builder;
    }
}
