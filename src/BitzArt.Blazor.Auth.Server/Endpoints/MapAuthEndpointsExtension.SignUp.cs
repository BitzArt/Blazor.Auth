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
    private static IEndpointRouteBuilder MapAuthSignUpEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/_auth/sign-up", async (
            [FromServices] AuthenticationServiceSignature authServiceSignature,
            [FromServices] IServiceProvider serviceProvider,
            [FromServices] IHttpContextAccessor httpContextAccessor,
            CancellationToken cancellationToken = default) =>
        {
            var payloadType = authServiceSignature.SignUpPayloadType;

            if (payloadType is null)
                return Results.BadRequest("The registered IAuthenticationService does not implement Sign-Up functionality.");

            var userService = serviceProvider.GetRequiredService<StaticUserService>()
                ?? throw new UnreachableException();

            var context = httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("The HttpContext is not available.");

            using StreamReader reader = new(context.Request.Body);
            var bodyAsString = await reader.ReadToEndAsync(cancellationToken);
            var payload = JsonSerializer.Deserialize(bodyAsString, payloadType, Constants.JsonSerializerOptions);

            if (payload is null) return Results.BadRequest("Invalid Sign-In payload.");

            var method = typeof(StaticUserService<>)
                .MakeGenericType(payloadType)
                .GetMethod(nameof(StaticUserService<object, object>.SignUpAsync))!;

            var info = await (Task<AuthenticationResultInfo>)method.Invoke(userService, [payload, cancellationToken])!;

            return Results.Ok(info);
        });

        return builder;
    }
}
