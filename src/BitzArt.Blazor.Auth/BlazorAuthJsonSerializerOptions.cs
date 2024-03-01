using System.Text.Json;
using System.Text.Json.Serialization;

namespace BitzArt.Blazor.Auth;

public static class BlazorAuthJsonSerializerOptions
{
    public static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
}
