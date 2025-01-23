using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Text.Json;

namespace BitzArt.Blazor.Auth.Server;

public static partial class MapAuthEndpointsExtension
{
    private static IEndpointRouteBuilder MapAuthMeEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/_auth/me", async (
            [FromServices] AuthenticationStateProvider authStateProvider,
            [FromServices] IHttpContextAccessor httpContextAccessor) =>
        {
            var state = await authStateProvider.GetAuthenticationStateAsync();
            var principal = state.User;
            var principalDto = principal.ToDto();
            var result = JsonSerializer.Serialize(principalDto, Constants.JsonSerializerOptions);

            return Results.Ok(result);
        });

        return builder;
    }
}
