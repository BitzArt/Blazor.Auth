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
    private static IEndpointRouteBuilder MapAuthSignInEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/_auth/sign-in", async (
            [FromServices] AuthenticationServiceSignature authServiceSignature,
            [FromServices] IServiceProvider serviceProvider,
            [FromServices] IHttpContextAccessor httpContextAccessor) =>
        {
            var payloadType = authServiceSignature.SignInPayloadType;

            if (payloadType is null)
                return Results.BadRequest("The registered IAuthenticationService does not implement Sign-In functionality.");

            var authService = serviceProvider.GetRequiredService<IAuthenticationService>()
                ?? throw new UnreachableException();

            var context = httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("The HttpContext is not available.");

            using StreamReader reader = new(context.Request.Body);
            var bodyAsString = await reader.ReadToEndAsync();
            var payload = JsonSerializer.Deserialize(bodyAsString, payloadType, Constants.JsonSerializerOptions);

            if (payload is null) return Results.BadRequest("Invalid Sign-In payload.");

            var method = typeof(IAuthenticationService<>)
                .MakeGenericType(payloadType)
                .GetMethod(nameof(IAuthenticationService<object>.SignInAsync))!;

            var result = await (Task<AuthenticationResult>)method.Invoke(authService, [payload])!;

            return Results.Ok(result);
        });

        return builder;
    }
}
