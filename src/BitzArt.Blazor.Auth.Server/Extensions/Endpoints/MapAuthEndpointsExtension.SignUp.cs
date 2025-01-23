using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Text.Json;

namespace BitzArt.Blazor.Auth.Server;

public static partial class MapAuthEndpointsExtension
{
    private static IEndpointRouteBuilder MapAuthSignUpEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/_auth/sign-up", async (
            [FromServices] IAuthenticationService authService,
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

        return builder;
    }
}
