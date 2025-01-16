using System.Net;

namespace BitzArt.Blazor.Auth.Client;

internal static class HttpResponseMessageExtensions
{
    internal static void Validate(this HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
            throw new NonSuccessResponseException(response.StatusCode);
    }
}

file class NonSuccessResponseException(HttpStatusCode statusCode) : Exception($"Server responded with status code '{statusCode}'.");
