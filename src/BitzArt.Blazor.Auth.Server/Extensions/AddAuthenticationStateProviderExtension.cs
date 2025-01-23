using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.Blazor.Auth.Server;

internal static class AddAuthenticationStateProviderExtension
{
    public static IServiceCollection AddServerSideAuthenticationStateProvider(this IServiceCollection services)
    {
        services.AddScoped<StaticAuthenticationStateProvider>();
        services.AddScoped<InteractiveAuthenticationStateProvider>();
        services.AddScoped(typeof(AuthenticationStateProvider), (serviceProvider, interactivityStatus) =>
        {
            return (AuthenticationStateProvider)(interactivityStatus.IsInteractive
                ? serviceProvider.GetRequiredService<InteractiveAuthenticationStateProvider>()
                : serviceProvider.GetRequiredService<StaticAuthenticationStateProvider>());
        });

        return services;
    }
}
