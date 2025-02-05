using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.Blazor.Auth.Server;

internal static class AddUserServiceExtension
{
    public static IServiceCollection AddUserService(this IServiceCollection services, AuthenticationServiceSignature authServiceSignature)
    {
        var staticServiceImplementationType = services.AddStaticUserService(authServiceSignature);
        var interactiveServiceImplementationType = services.AddInteractiveUserService(authServiceSignature);

        var globalInterfaces = GetGlobalRegistrationInterfaces(authServiceSignature);

        foreach (var globalInterface in globalInterfaces)
        {
            services.AddScoped(globalInterface, (serviceProvider, interactivityStatus) =>
            {
                return interactivityStatus.IsInteractive
                    ? serviceProvider.GetRequiredService(interactiveServiceImplementationType)
                    : serviceProvider.GetRequiredService(staticServiceImplementationType);
            });
        }

        return services;
    }

    private static Type AddStaticUserService(this IServiceCollection services, AuthenticationServiceSignature authServiceSignature)
    {
        var registrationInfo = StaticUserService.GetServiceRegistrationInfo(authServiceSignature);

        services.AddScoped(registrationInfo.ImplementationType);
        if (registrationInfo.AdditionalTypes is null) return registrationInfo.ImplementationType;

        foreach (var additionalType in registrationInfo.AdditionalTypes)
        {
            services.AddScoped(additionalType, x => x.GetRequiredService(registrationInfo.ImplementationType));
        }

        return registrationInfo.ImplementationType;
    }

    private static Type AddInteractiveUserService(this IServiceCollection services, AuthenticationServiceSignature authServiceSignature)
    {
        var registrationInfo = InteractiveUserService.GetServiceRegistrationInfo(authServiceSignature);

        services.AddScoped(registrationInfo.ImplementationType);
        if (registrationInfo.AdditionalTypes is null) return registrationInfo.ImplementationType;

        foreach (var additionalType in registrationInfo.AdditionalTypes)
        {
            services.AddScoped(additionalType, x => x.GetRequiredService(registrationInfo.ImplementationType));
        }

        return registrationInfo.ImplementationType;
    }

    private static List<Type> GetGlobalRegistrationInterfaces(AuthenticationServiceSignature signature)
    {
        var basic = typeof(IUserService);

        var signIn = signature.SignInPayloadType is null
            ? null
            : typeof(IUserService<>).MakeGenericType(signature.SignInPayloadType);

        if (signIn is null) return [basic];

        var signUp = signature.SignUpPayloadType is null
            ? null
            : typeof(IUserService<,>).MakeGenericType(signature.SignUpPayloadType, signature.SignUpPayloadType);

        if (signUp is null) return [basic, signIn];

        return [basic, signIn, signUp];
    }
}
