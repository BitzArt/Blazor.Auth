using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BitzArt.Blazor.Auth.Server;

public static partial class MapAuthEndpointsExtension
{
    private static IEndpointRouteBuilder MapAuthSignOutEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/_auth/sign-out", async (
            IUserService userService,
            CancellationToken cancellationToken = default) =>
        {
            await userService.SignOutAsync(cancellationToken);
            return Results.Ok();
        });

        return builder;
    }
}
