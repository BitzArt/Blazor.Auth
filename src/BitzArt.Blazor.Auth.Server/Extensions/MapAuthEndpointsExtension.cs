using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Text.Json;

namespace BitzArt.Blazor.Auth;

public static class MapAuthEndpointsExtension
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/api/sign-in", async (
            IAuthenticationService authService,
            [FromServices] IHttpContextAccessor httpContextAccessor) =>
        {
            var type = authService.GetSignInPayloadType();

            if (type is null) throw new NotImplementedException();

            var context = httpContextAccessor.HttpContext;
            using StreamReader reader = new(context!.Request.Body);
            var bodyAsString = await reader.ReadToEndAsync();

            var payload = JsonSerializer.Deserialize(bodyAsString, type);
            var result = await authService.GetSignInResultAsync(payload!);

            return Results.Ok(result);
        });

        builder.MapPost("/api/sign-up", async (
            IAuthenticationService authService,
            [FromServices] IHttpContextAccessor httpContextAccessor) =>
        {
            var type = authService.GetSignUpPayloadType();

            if (type is null) throw new NotImplementedException();

            var context = httpContextAccessor.HttpContext;
            using StreamReader reader = new(context!.Request.Body);
            var bodyAsString = await reader.ReadToEndAsync();

            var payload = JsonSerializer.Deserialize(bodyAsString, type);
            var result = await authService.GetSignUpResultAsync(payload!);

            return Results.Ok(result);
        });

        builder.MapPost("/api/refresh", async (
            IAuthenticationService authService, [FromServices] IHttpContextAccessor httpContextAccessor) =>
        {
            var context = httpContextAccessor.HttpContext;
            using StreamReader reader = new(context!.Request.Body);
            var bodyAsString = await reader.ReadToEndAsync();

            var refreshToken = JsonSerializer.Deserialize<string>(bodyAsString);
            var result = await authService.GetRefreshJwtPairResultAsync(refreshToken!);

            return result;
        });

        return builder;
    }
}
