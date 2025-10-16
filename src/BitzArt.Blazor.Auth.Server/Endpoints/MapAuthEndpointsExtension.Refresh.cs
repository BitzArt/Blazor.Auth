using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Text.Json;

namespace BitzArt.Blazor.Auth.Server;

public static partial class MapAuthEndpointsExtension
{
    private static IEndpointRouteBuilder MapAuthRefreshEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/_auth/refresh", async (
            [FromServices] IServiceProvider serviceProvider,
            [FromServices] IHttpContextAccessor httpContextAccessor,
            CancellationToken cancellationToken = default) =>
        {
            var userService = serviceProvider.GetRequiredService<StaticUserService>();

            var context = httpContextAccessor.HttpContext
                    ?? throw new InvalidOperationException("The HttpContext is not available.");

            using StreamReader reader = new(context.Request.Body);
            var bodyAsString = await reader.ReadToEndAsync(cancellationToken);

            var refreshToken = string.IsNullOrWhiteSpace(bodyAsString)
                ? null
                : JsonSerializer.Deserialize<string?>(bodyAsString, Constants.JsonSerializerOptions);

            var result = refreshToken switch
            {
                null => await userService.RefreshJwtPairAsync(cancellationToken),
                string => await userService.RefreshJwtPairAsync(refreshToken, cancellationToken)
            };

            return Results.Ok(result);
        });

        return builder;
    }
}
