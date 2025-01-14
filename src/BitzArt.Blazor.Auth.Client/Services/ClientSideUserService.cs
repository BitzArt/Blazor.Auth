using System.Net.Http.Json;
using System.Text.Json;

namespace BitzArt.Blazor.Auth.Client;

internal class ClientSideUserService(HttpClient httpClient) : IUserService
{
    public async Task<AuthenticationResult> SignInAsync(object signInPayload)
    {
        var response = await httpClient.PostAsJsonAsync("/_auth/sign-in", signInPayload, Constants.JsonSerializerOptions);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Server responded with status code '{response.StatusCode}'.");

        var content = await response.Content.ReadAsStringAsync();
        var authResult = JsonSerializer.Deserialize<AuthenticationResult>(content, Constants.JsonSerializerOptions);

        return authResult is null
            ? throw new Exception("Server responded with null authentication result.")
            : authResult;
    }

    public async Task<AuthenticationResult> SignUpAsync(object signUpPayload)
    {
        var response = await httpClient.PostAsJsonAsync("/_auth/sign-up", signUpPayload, Constants.JsonSerializerOptions);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Server responded with status code '{response.StatusCode}'.");

        var content = await response.Content.ReadAsStringAsync();
        var authResult = JsonSerializer.Deserialize<AuthenticationResult>(content, Constants.JsonSerializerOptions);

        return authResult is null
            ? throw new Exception("Server responded with null authentication result.")
            : authResult;
    }

    public async Task<AuthenticationResult> RefreshJwtPairAsync(string refreshToken)
    {
        var response = await httpClient.PostAsJsonAsync("/_auth/refresh", refreshToken, Constants.JsonSerializerOptions);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Server responded with status code '{response.StatusCode}'.");

        var content = await response.Content.ReadAsStringAsync();
        var authResult = JsonSerializer.Deserialize<AuthenticationResult>(content, Constants.JsonSerializerOptions);

        return authResult is null
            ? throw new Exception("Server responded with null authentication result.")
            : authResult;
    }

    public async Task SignOutAsync()
    {
        var response = await httpClient.PostAsync("/_auth/sign-out", null);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Server responded with status code '{response.StatusCode}'.");
    }
}
