using System.Text.Json.Serialization;
using System.Text.Json;

namespace BitzArt.Blazor.Auth;

internal static class Constants
{
    public const string AccessTokenCookieName = "AccessToken";
    public const string RefreshTokenCookieName = "RefreshToken";

    public static class SerializerOptions
    {
        public static readonly JsonSerializerOptions Options = new()
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
    }
}
