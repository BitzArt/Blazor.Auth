using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BitzArt.Blazor.Auth;

public static class AddBlazorServerAuthExtension
{
    public static IServiceCollection AddBlazorServerAuth(this IServiceCollection services)
    {
        return services.AddBlazorServerAuth<IdentityClaimsService, ServerSideAuthenticationService<object, object>>();
    }
    public static IServiceCollection AddBlazorServerAuth<TServerSideAuthenticationService>(this IServiceCollection services)
        where TServerSideAuthenticationService : class, IServerSideAuthenticationService
    {
        return services.AddBlazorServerAuth<IdentityClaimsService, TServerSideAuthenticationService>();
    }

    public static IServiceCollection AddBlazorServerAuth<TIdentityClaimsService, TServerSideAuthenticationService>(this IServiceCollection services)
        where TIdentityClaimsService : class, IIdentityClaimsService
        where TServerSideAuthenticationService : class, IServerSideAuthenticationService
    {
        services.AddHttpContextAccessor();
        services.AddCascadingAuthenticationState();
        services.AddBlazoredLocalStorage();

        services.AddScoped<IIdentityClaimsService, TIdentityClaimsService>();
        services.AddScoped<AuthenticationStateProvider, BlazorAuthenticationStateProvider>();
        services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddlewareResultHandler>();

        services.ConfigureHttpJsonOptions(opts =>
        {
            opts.SerializerOptions.PropertyNameCaseInsensitive = true;
            opts.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            opts.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

        // UserService
        services.AddScoped<IUserService, UserService>();

        // AuthenticationService
        services.AddScoped<IAuthenticationService, TServerSideAuthenticationService>();
        services.AddScoped(sp => (IServerSideAuthenticationService)sp.GetRequiredService<IAuthenticationService>());

        return services;
    }
}
