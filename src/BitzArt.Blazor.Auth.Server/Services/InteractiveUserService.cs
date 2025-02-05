using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace BitzArt.Blazor.Auth.Server;

// Interactive server-side implementation of the user service.
internal class InteractiveUserService(
    IBlazorAuthLogger logger,
    NavigationManager navigation,
    IJSRuntime js) : IUserService
{
    private protected readonly IBlazorAuthLogger Logger = logger;
    private protected readonly NavigationManager Navigation = navigation;
    private protected readonly IJSRuntime Js = js;

    public async Task<AuthenticationState> GetAuthenticationStateAsync(CancellationToken cancellationToken = default)
        => await DoWhileLogging(async ()
            => await DoWithJsModule(async (module)
                =>
                {
                    var baseUri = GetBaseUri();
                    var url = $"{baseUri.TrimEnd('/')}/_auth/me";

                    var response = await module.InvokeAsync<ClaimsPrincipalDto>(
                        "requestAsync",
                        cancellationToken: cancellationToken,
                        [url, HttpMethod.Get.Method, null, "json"]);

                    var principal = response.ToModel();

                    return new AuthenticationState(principal);
                }));

    public async Task<AuthenticationOperationInfo> RefreshJwtPairAsync(string refreshToken, CancellationToken cancellationToken = default)
        => await DoWhileLogging(async ()
            => await DoWithJsModule(async (module)
                =>
                {
                    var baseUri = GetBaseUri();
                    var url = $"{baseUri.TrimEnd('/')}/_auth/refresh";

                    var result = await module.InvokeAsync<AuthenticationOperationInfo>(
                        "requestAsync",
                        cancellationToken: cancellationToken,
                        [url, HttpMethod.Post.Method, refreshToken, "json"])
                        ?? throw new InvalidOperationException("Failed to deserialize the authentication result info.");

                    return result;
                }));

    public async Task SignOutAsync(CancellationToken cancellationToken = default)
        => await DoWhileLogging(async ()
            => await DoWithJsModule(async (module)
                =>
                {
                    var baseUri = GetBaseUri();
                    var url = $"{baseUri.TrimEnd('/')}/_auth/sign-out";

                    await module.InvokeVoidAsync(
                        "requestAsync",
                        cancellationToken: cancellationToken,
                        [url, HttpMethod.Post.Method, null, null]);

                    return true;
                }));

    private protected string GetBaseUri()
    {
        var baseUri = Navigation.BaseUri;
        Logger.LogDebug("BaseUri: {baseUri}", baseUri);
        return baseUri;
    }

    private protected async Task<T> DoWithJsModule<T>(Func<IJSObjectReference, Task<T>> action)
    {
        var module = await Js.InvokeAsync<IJSObjectReference>("import", "./_content/BitzArt.Blazor.Auth.Server/auth.js");

        try
        {
            var actionTask = action.Invoke(module);
            return await actionTask;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, AuthRequestFailedException.ErrorMessage);
            throw new AuthRequestFailedException(ex);
        }
        finally
        {
            await module.DisposeAsync();
        }
    }

    private protected async Task<T> DoWhileLogging<T>(Func<Task<T>> action, [CallerMemberName] string operationName = "")
    {
        Logger.LogDebug("{operationName} was called.", operationName);

        try
        {
            var actionTask = action.Invoke();
            var result = await actionTask;

            Logger.LogDebug("{operationName} completed.", operationName);

            return result;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while executing the action.");
            throw;
        }
    }

    internal static UserServiceRegistrationInfo GetServiceRegistrationInfo(AuthenticationServiceSignature signature)
    {
        if (signature.SignInPayloadType is null)
            return new(GetServiceBasicType());

        if (signature.SignUpPayloadType is null)
            return new(GetServiceSignInType(signature),
                [GetServiceBasicType()]);

        return new(GetServiceSignUpType(signature),
            [GetServiceBasicType(), GetServiceSignInType(signature)]);
    }

    private static Type GetServiceBasicType() => typeof(InteractiveUserService);
    private static Type GetServiceSignInType(AuthenticationServiceSignature signature) => typeof(InteractiveUserService<>).MakeGenericType(signature.SignInPayloadType!);
    private static Type GetServiceSignUpType(AuthenticationServiceSignature signature) => typeof(InteractiveUserService<,>).MakeGenericType(signature.SignInPayloadType!, signature.SignUpPayloadType!);
}

internal class InteractiveUserService<TSignInPayload>(
    IBlazorAuthLogger logger,
    NavigationManager navigation,
    IJSRuntime js
    ) : InteractiveUserService(logger, navigation, js), IUserService<TSignInPayload>
{
    public async Task<AuthenticationOperationInfo> SignInAsync(TSignInPayload signInPayload, CancellationToken cancellationToken = default)
        => await DoWhileLogging(async ()
            => await DoWithJsModule(async (module)
                =>
                {
                    var baseUri = GetBaseUri();
                    var url = $"{baseUri.TrimEnd('/')}/_auth/sign-in";

                    var result = await module.InvokeAsync<AuthenticationOperationInfo>(
                        "requestAsync",
                        cancellationToken: cancellationToken,
                        [url, HttpMethod.Post.Method, signInPayload, "json"])
                        ?? throw new InvalidOperationException("Failed to deserialize the authentication result info.");

                    return result;
                }));
}

internal class InteractiveUserService<TSignInPayload, TSignUpPayload>(
    IBlazorAuthLogger logger,
    NavigationManager navigation,
    IJSRuntime js) : InteractiveUserService<TSignInPayload>(logger, navigation, js), IUserService<TSignInPayload, TSignUpPayload>
{
    public async Task<AuthenticationOperationInfo> SignUpAsync(TSignUpPayload signUpPayload, CancellationToken cancellationToken = default)
        => await DoWhileLogging(async ()
            => await DoWithJsModule(async (module)
                =>
                {
                    var baseUri = GetBaseUri();
                    var url = $"{baseUri.TrimEnd('/')}/_auth/sign-up";

                    var result = await module.InvokeAsync<AuthenticationOperationInfo>(
                        "requestAsync",
                        cancellationToken: cancellationToken,
                        [url, HttpMethod.Post.Method, signUpPayload, "json"])
                        ?? throw new InvalidOperationException("Failed to deserialize the authentication result info.");

                    return result;
                }));
}
