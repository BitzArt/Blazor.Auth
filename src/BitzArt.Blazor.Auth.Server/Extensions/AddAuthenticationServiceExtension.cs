using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.Blazor.Auth.Server;

internal static class AddAuthenticationServiceExtension
{
    public static AuthenticationServiceSignature AddAuthenticationService<TAuthenticationService>(this IServiceCollection services)
        where TAuthenticationService : class, IAuthenticationService
    {
        services.AddScoped<TAuthenticationService>();

        var signature = GetSignature(typeof(TAuthenticationService));
        services.AddSingleton(signature);

        var registrationInterfaces = GetRegistrationInterfaces(signature);
        foreach (var registration in registrationInterfaces) services.AddScoped(registration, x => x.GetRequiredService<TAuthenticationService>());

        return signature;
    }

    private static AuthenticationServiceSignature GetSignature(Type serviceType)
    {
        var signInPayloadType = serviceType.GetInterfaces()
            .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IAuthenticationService<>))
            .Select(x => x.GetGenericArguments()[0])
            .FirstOrDefault();

        if (signInPayloadType is null) return new AuthenticationServiceSignature(null, null);

        var signUpPayloadType = serviceType.GetInterfaces()
            .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IAuthenticationService<,>))
            .Select(x => x.GetGenericArguments()[1])
            .FirstOrDefault();

        if (signUpPayloadType is null) return new AuthenticationServiceSignature(signInPayloadType, null);

        return new AuthenticationServiceSignature(signInPayloadType, signUpPayloadType);
    }

    private static List<Type> GetRegistrationInterfaces(AuthenticationServiceSignature signature)
    {
        var basic = typeof(IAuthenticationService);

        var signIn = signature.SignInPayloadType is null
            ? null
            : typeof(IAuthenticationService<>).MakeGenericType(signature.SignInPayloadType);

        if (signIn is null) return [basic];

        var signUp = signature.SignUpPayloadType is null
            ? null
            : typeof(IAuthenticationService<,>).MakeGenericType(signature.SignInPayloadType!, signature.SignUpPayloadType);

        if (signUp is null) return [basic, signIn];

        return [basic, signIn, signUp];
    }
}
