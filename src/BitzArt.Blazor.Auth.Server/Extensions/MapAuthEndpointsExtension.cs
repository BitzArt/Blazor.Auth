using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Text.Json;

namespace BitzArt.Blazor.Auth.Server;

/// <summary>
/// Extension methods for mapping server-side authentication endpoints.
/// </summary>
public static class MapAuthEndpointsExtension
{
    /// <summary>
    /// Maps the server-side endpoints necessary for client-side authentication to work.
    /// </summary>
    /// <param name="builder"> The <see cref="IEndpointRouteBuilder"/>. </param>
    /// <returns> The <see cref="IEndpointRouteBuilder"/> to allow chaining. </returns>
    /// <exception cref="NotImplementedException"></exception>
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/_auth/me", async (
            AuthenticationStateProvider authStateProvider,
            [FromServices] IHttpContextAccessor httpContextAccessor) =>
        {
            var state = await authStateProvider.GetAuthenticationStateAsync();
            var result = state.ToDto();

            return Results.Ok(result);
        });

        builder.MapPost("/_auth/sign-in", async (
            IServerSideAuthenticationService authService,
            [FromServices] IHttpContextAccessor httpContextAccessor) =>
        {
            var type = authService.GetSignInPayloadType() ?? throw new NotImplementedException();

            var context = httpContextAccessor.HttpContext;
            using StreamReader reader = new(context!.Request.Body);
            var bodyAsString = await reader.ReadToEndAsync();
            var payload = JsonSerializer.Deserialize(bodyAsString, type, Constants.JsonSerializerOptions);

            var result = await authService.SignInAsync(payload!);

            return Results.Ok(result);
        });

        builder.MapPost("/_auth/sign-up", async (
            IServerSideAuthenticationService authService,
            [FromServices] IHttpContextAccessor httpContextAccessor) =>
        {
            var type = authService.GetSignUpPayloadType() ?? throw new NotImplementedException();

            var context = httpContextAccessor.HttpContext;
            using StreamReader reader = new(context!.Request.Body);
            var bodyAsString = await reader.ReadToEndAsync();
            var payload = JsonSerializer.Deserialize(bodyAsString, type, Constants.JsonSerializerOptions);

            var result = await authService.SignUpAsync(payload!);

            return Results.Ok(result);
        });

        builder.MapPost("/_auth/refresh", async (
            IServerSideAuthenticationService authService,
            [FromServices] IHttpContextAccessor httpContextAccessor) =>
        {
            var context = httpContextAccessor.HttpContext;
            using StreamReader reader = new(context!.Request.Body);
            var bodyAsString = await reader.ReadToEndAsync();
            var refreshToken = JsonSerializer.Deserialize<string>(bodyAsString, Constants.JsonSerializerOptions);

            var result = await authService.RefreshJwtPairAsync(refreshToken!);

            return result;
        });

        return builder;
    }
}
