using System.Text.Json;
using System.Text.Json.Serialization;

namespace BitzArt.Blazor.Auth;

internal static class Constants
{
    public static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
}
