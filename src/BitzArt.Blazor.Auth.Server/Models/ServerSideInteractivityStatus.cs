using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.Blazor.Auth.Server;

/// <summary>
/// Determines server-side interactivity status. <br />
/// Static SSR and WASM rendering will result in <see cref="IsInteractive"/> being <see langword="false"/>. <br />
/// InteractiveServerSideRendering will result in <see cref="IsInteractive"/> being <see langword="true"/>.
/// </summary>
internal class ServerSideInteractivityStatus(bool isInteractive)
{
    public bool IsInteractive { get; init; } = isInteractive;
}

internal static class ServiceCollectionInteractivityStatusExtensions
{

    public static IServiceCollection AddScoped<TService>(this IServiceCollection services, Type serviceType, Func<IServiceProvider, ServerSideInteractivityStatus, TService> implementationFactory)
        where TService : class
        => services.AddScoped(serviceType, serviceProvider =>
        {
            var interactivityStatus = serviceProvider.GetRequiredService<ServerSideInteractivityStatus>();
            return implementationFactory(serviceProvider, interactivityStatus);
        });

    public static IServiceCollection AddServerSideInteractivityStatus(this IServiceCollection services)
        => services.AddScoped(serviceProvider =>
        {
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("The HttpContext is not available.");

            var isInteractive = httpContext.Response.HasStarted;

            return new ServerSideInteractivityStatus(isInteractive);
        });
}
