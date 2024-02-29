using Blazored.LocalStorage;
using System.Net.Http.Json;
using System.Text.Json;

namespace BitzArt.Blazor.Auth;

public class ClientSideAuthenticationService(
    ILocalStorageService localStorage,
    HttpClient httpClient)
    : AuthenticationService(localStorage)
{
    public override async Task<AuthenticationResult?> GetSignInResultAsync(object signInPayload)
    {
        var response = await httpClient.PostAsJsonAsync("/api/sign-in", signInPayload);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Server responded with status code '{response.StatusCode}'");

        var content = await response.Content.ReadAsStringAsync();
        var authResult = JsonSerializer.Deserialize<AuthenticationResult>(content, BlazorAuthJsonSerializerOptions.GetOptions());

        return authResult;
    }

    public override async Task<AuthenticationResult?> GetSignUpResultAsync(object signUpPayload)
    {
        var response = await httpClient.PostAsJsonAsync("/api/sign-up", signUpPayload);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Server responded with status code '{response.StatusCode}'");

        var content = await response.Content.ReadAsStringAsync();
        var authResult = JsonSerializer.Deserialize<AuthenticationResult>(content, BlazorAuthJsonSerializerOptions.GetOptions());

        return authResult;
    }

    public override async Task<AuthenticationResult?> GetRefreshJwtPairResultAsync(string refreshToken)
    {
        var response = await httpClient.PostAsJsonAsync("/api/refresh", refreshToken);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Server responded with status code '{response.StatusCode}'");

        var content = await response.Content.ReadAsStringAsync();
        var authResult = JsonSerializer.Deserialize<AuthenticationResult>(content, BlazorAuthJsonSerializerOptions.GetOptions());

        return authResult;
    }
}
