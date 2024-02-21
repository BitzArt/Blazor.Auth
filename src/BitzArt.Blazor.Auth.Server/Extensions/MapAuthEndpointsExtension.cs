using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace BitzArt.Blazor.Auth;

public static class MapAuthEndpointsExtension
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/api/sign-in", async (
            IAuthenticationService authService, [FromServices] IHttpContextAccessor httpContextAccessor ) =>
        {
            var type = authService.GetSignInPayloadType();

            if (type is null) throw new NotImplementedException();

            var context = httpContextAccessor.HttpContext;

            using StreamReader reader = new(context!.Request.Body);
            var bodyAsString = await reader.ReadToEndAsync();

            var payload = JsonSerializer.Deserialize(bodyAsString, type);

            var result = await authService.GetJwtPairAsync(payload!);

            return Results.Ok(result);
        });

        builder.MapPost("/api/refresh", async (
            IAuthenticationService authService, [FromServices] IHttpContextAccessor httpContextAccessor) =>
        {
            var context = httpContextAccessor.HttpContext;

            using StreamReader reader = new(context!.Request.Body);
            var bodyAsString = await reader.ReadToEndAsync();

            var refreshToken = JsonSerializer.Deserialize<string>(bodyAsString);

            var result = await authService.RefreshJwtPairAsync(refreshToken!);

            return result;
        });

        return builder;
    }
}