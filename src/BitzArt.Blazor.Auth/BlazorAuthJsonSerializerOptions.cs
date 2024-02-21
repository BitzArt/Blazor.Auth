using System.Text.Json;
using System.Text.Json.Serialization;

namespace BitzArt.Blazor.Auth;

public static class BlazorAuthJsonSerializerOptions 
{
    private static JsonSerializerOptions _jsonSerializerOptions;

    public static JsonSerializerOptions GetOptions()
    {
        _jsonSerializerOptions ??= new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        return _jsonSerializerOptions;
    }
}
