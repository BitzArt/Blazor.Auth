using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace BitzArt.Blazor.Auth.Client;

internal class BlazorHostHttpClient(HttpClient httpClient, IBlazorAuthLogger logger)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IBlazorAuthLogger _logger = logger;

    public HttpClient HttpClient => _httpClient;

    private const string _errorMessage = $"Failed to parse authentication response from the host. See inner exception for details.";

    public async Task<TResponse> GetAsync<TResponse>(string requestUri, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(requestUri, cancellationToken);
        return await ParseResponseAsync<TResponse>(response, cancellationToken);
    }

    public async Task<TResponse> PostAsync<TResponse>(string requestUri, object value, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(requestUri, value, Constants.JsonSerializerOptions, cancellationToken);
        var result = await ParseResponseAsync<TResponse>(response, cancellationToken);

        return result;
    }

    public async Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken = default)
        => await _httpClient.PostAsync(requestUri, null, cancellationToken);

    public async Task<HttpResponseMessage> PostAsync(string requestUri, CancellationToken cancellationToken = default)
        => await _httpClient.PostAsync(requestUri, null, cancellationToken);

    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
        => await _httpClient.SendAsync(request, cancellationToken);

    private async Task<TResult> ParseResponseAsync<TResult>(HttpResponseMessage response, CancellationToken cancellationToken = default)
    {
        try
        {
            response.Validate();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(content))
                throw new EmptyResponseException();

            return Deserialize<TResult>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _errorMessage);
            throw new AuthRequestFailedException(_errorMessage, ex);
        }
    }

    private static TResult Deserialize<TResult>(string content)
    {
        try
        {
            var result = JsonSerializer.Deserialize<TResult>(content, Constants.JsonSerializerOptions)
                ?? throw new Exception("resulting object is null.");

            return result;
        }
        catch (Exception ex)
        {
            throw new ResponseDeserializationException<TResult>(ex);
        }
    }
}

file class EmptyResponseException() : Exception("Server responded with an empty body.");

file class ResponseDeserializationException<T>(Exception? innerException = null)
    : Exception($"Failed to deserialize response body to type '{typeof(T).Name}'.", innerException);
