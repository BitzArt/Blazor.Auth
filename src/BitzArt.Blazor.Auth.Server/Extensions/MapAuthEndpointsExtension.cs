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
            IServerSideAuthenticationService authService,
            [FromServices] IHttpContextAccessor httpContextAccessor) =>
        {
            var type = authService.GetSignInPayloadType() ?? throw new NotImplementedException();

            var context = httpContextAccessor.HttpContext;
            using StreamReader reader = new(context!.Request.Body);
            var bodyAsString = await reader.ReadToEndAsync();
            var payload = JsonSerializer.Deserialize(bodyAsString, type, BlazorAuthJsonSerializerOptions.Options);

            var result = await authService.SignInAsync(payload!);

            return Results.Ok(result);
        });

        builder.MapPost("/api/sign-up", async (
            IServerSideAuthenticationService authService,
            [FromServices] IHttpContextAccessor httpContextAccessor) =>
        {
            var type = authService.GetSignUpPayloadType() ?? throw new NotImplementedException();

            var context = httpContextAccessor.HttpContext;
            using StreamReader reader = new(context!.Request.Body);
            var bodyAsString = await reader.ReadToEndAsync();
            var payload = JsonSerializer.Deserialize(bodyAsString, type, BlazorAuthJsonSerializerOptions.Options);

            var result = await authService.SignUpAsync(payload!);

            return Results.Ok(result);
        });

        builder.MapPost("/api/refresh", async (
            IServerSideAuthenticationService authService,
            [FromServices] IHttpContextAccessor httpContextAccessor) =>
        {
            var context = httpContextAccessor.HttpContext;
            using StreamReader reader = new(context!.Request.Body);
            var bodyAsString = await reader.ReadToEndAsync();
            var refreshToken = JsonSerializer.Deserialize<string>(bodyAsString, BlazorAuthJsonSerializerOptions.Options);

            var result = await authService.RefreshJwtPairAsync(refreshToken!);

            return result;
        });

        return builder;
    }
}
