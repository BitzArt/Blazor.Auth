using Microsoft.AspNetCore.Routing;

namespace BitzArt.Blazor.Auth.Server;

/// <summary>
/// Extension methods for mapping server-side authentication endpoints.
/// </summary>
public static partial class MapAuthEndpointsExtension
{
    /// <summary>
    /// Maps the server-side endpoints necessary for client-side authentication to work.
    /// </summary>
    /// <param name="builder"> The <see cref="IEndpointRouteBuilder"/>. </param>
    /// <returns> The <see cref="IEndpointRouteBuilder"/> to allow chaining. </returns>
    /// <exception cref="NotImplementedException"></exception>
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapAuthMeEndpoint();
        builder.MapAuthRefreshEndpoint();

        builder.MapAuthSignInEndpoint();
        builder.MapAuthSignUpEndpoint();
        builder.MapAuthSignOutEndpoint();

        return builder;
    }
}
