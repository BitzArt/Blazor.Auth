﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.Blazor.Auth;

public static class AddBlazorAuthExtension
{
    public static IServiceCollection AddBlazorClientAuth(this IServiceCollection services)
    {
        return services.AddBlazorClientAuth<ClientSideAuthenticationService>();
    }

    public static IServiceCollection AddBlazorClientAuth<TAuthenticationService>(this IServiceCollection services)
        where TAuthenticationService : class, IAuthenticationService
    {
        return services.AddBlazorClientAuth<TAuthenticationService, IdentityClaimsService>();
    }

    public static IServiceCollection AddBlazorClientAuth<TAuthenticationService, TIdentityClaimsService>(this IServiceCollection services)
        where TAuthenticationService : class, IAuthenticationService
        where TIdentityClaimsService : class, IIdentityClaimsService
    {
        services.AddAuthorizationCore();
        services.AddCascadingAuthenticationState();
        services.AddBlazoredLocalStorage();

        services.AddScoped<IIdentityClaimsService, TIdentityClaimsService>();
        services.AddScoped<AuthenticationStateProvider, BlazorAuthenticationStateProvider>();

        // UserService
        services.AddScoped<IUserService, UserService>();

        // AuthenticationService
        services.AddScoped<IAuthenticationService, TAuthenticationService>();

        return services;
    }
}
