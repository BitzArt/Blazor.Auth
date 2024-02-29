using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BitzArt.Blazor.Auth;

public static class AddBlazorAuthExtension
{
    public static IServiceCollection AddBlazorServerAuth<TServerSideAuthenticationService>(this IServiceCollection services)
        where TServerSideAuthenticationService : class, IAuthenticationService
    {
        return services.AddBlazorServerAuth<IdentityClaimsService, TServerSideAuthenticationService>();
    }

    public static IServiceCollection AddBlazorServerAuth<TIdentityClaimsService, TServerSideAuthenticationService>(this IServiceCollection services)
        where TIdentityClaimsService : class, IIdentityClaimsService
        where TServerSideAuthenticationService : class, IAuthenticationService
    {
        services.AddHttpContextAccessor();
        services.AddCascadingAuthenticationState();
        services.AddBlazoredLocalStorage();

        services.AddScoped<IIdentityClaimsService, TIdentityClaimsService>();
        services.AddScoped<IAuthenticationService, TServerSideAuthenticationService>();
        services.AddScoped<AuthenticationStateProvider, BlazorAuthenticationStateProvider>();
        services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddlewareResultHandler>();

        services.ConfigureHttpJsonOptions(opts =>
        {
            opts.SerializerOptions.PropertyNameCaseInsensitive = true;
            opts.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            opts.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

        return services;
    }
}
