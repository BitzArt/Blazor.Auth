using Blazored.LocalStorage;
using System.Net.Http.Json;
using System.Text.Json;

namespace BitzArt.Blazor.Auth;

public class ClientSideAuthenticationService(ILocalStorageService localStorage, HttpClient httpClient) 
    : AuthenticationService(localStorage)
{
    public override async Task<JwtPair?> GetJwtPairAsync(object signInPayload)
    {
        var response = await httpClient.PostAsJsonAsync("/api/sign-in", signInPayload);

        if (!response.IsSuccessStatusCode) 
            throw new Exception($"Server responded with status code '{response.StatusCode}'");

        var content = await response.Content.ReadAsStringAsync();

        var jwtPair = JsonSerializer.Deserialize<JwtPair>(content, BlazorAuthJsonSerializerOptions.GetOptions());
        
        return jwtPair;
    }

    public override async Task<JwtPair?> RefreshJwtPairAsync(string refreshToken)
    {
        var response = await httpClient.PostAsJsonAsync("/api/refresh", refreshToken);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Server responded with status code '{response.StatusCode}'");

        var content = await response.Content.ReadAsStringAsync();

        var jwtPair = JsonSerializer.Deserialize<JwtPair>(content, BlazorAuthJsonSerializerOptions.GetOptions());

        return jwtPair;
    }
}
