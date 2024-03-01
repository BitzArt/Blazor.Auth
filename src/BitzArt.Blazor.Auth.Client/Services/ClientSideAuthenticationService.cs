using System.Net.Http.Json;
using System.Text.Json;

namespace BitzArt.Blazor.Auth;

public class ClientSideAuthenticationService(
    HttpClient httpClient)
    : AuthenticationService
{
    public override async Task<AuthenticationResult> SignInAsync(object signInPayload)
    {
        var response = await httpClient.PostAsJsonAsync("/api/sign-in", signInPayload);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Server responded with status code '{response.StatusCode}'");

        var content = await response.Content.ReadAsStringAsync();
        var authResult = JsonSerializer.Deserialize<AuthenticationResult>(content, BlazorAuthJsonSerializerOptions.Options);

        return authResult is null
            ? throw new Exception("Server responded with null authentication result")
            : authResult;
    }

    public override async Task<AuthenticationResult> SignUpAsync(object signUpPayload)
    {
        var response = await httpClient.PostAsJsonAsync("/api/sign-up", signUpPayload);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Server responded with status code '{response.StatusCode}'");

        var content = await response.Content.ReadAsStringAsync();
        var authResult = JsonSerializer.Deserialize<AuthenticationResult>(content, BlazorAuthJsonSerializerOptions.Options);

        return authResult is null
            ? throw new Exception("Server responded with null authentication result")
            : authResult;
    }

    public override async Task<AuthenticationResult> RefreshJwtPairAsync(string refreshToken)
    {
        var response = await httpClient.PostAsJsonAsync("/api/refresh", refreshToken);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Server responded with status code '{response.StatusCode}'");

        var content = await response.Content.ReadAsStringAsync();
        var authResult = JsonSerializer.Deserialize<AuthenticationResult>(content, BlazorAuthJsonSerializerOptions.Options);

        return authResult is null
            ? throw new Exception("Server responded with null authentication result")
            : authResult;
    }
}
