﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Text.Json;

namespace BitzArt.Blazor.Auth.Server;

public static partial class MapAuthEndpointsExtension
{
    private static IEndpointRouteBuilder MapAuthRefreshEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/_auth/refresh", async (
            [FromServices] IAuthenticationService authService,
            [FromServices] IHttpContextAccessor httpContextAccessor,
            CancellationToken cancellationToken = default) =>
        {
            var context = httpContextAccessor.HttpContext;
            using StreamReader reader = new(context!.Request.Body);
            var bodyAsString = await reader.ReadToEndAsync(cancellationToken);
            var refreshToken = JsonSerializer.Deserialize<string>(bodyAsString, Constants.JsonSerializerOptions);

            var result = await authService.RefreshJwtPairAsync(refreshToken!, cancellationToken);
            var info = result.GetInfo();

            return Results.Ok(info);
        });

        return builder;
    }
}
