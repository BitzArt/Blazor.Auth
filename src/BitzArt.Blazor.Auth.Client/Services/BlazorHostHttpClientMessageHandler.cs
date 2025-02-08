using Microsoft.Extensions.Logging;

namespace BitzArt.Blazor.Auth.Client;

internal class BlazorHostHttpClientMessageHandler(IBlazorAuthLogger logger)
    : DelegatingHandler
{
    private readonly ILogger _logger = logger;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug("{method} {uri}", request.Method.ToString(), request.RequestUri);

            return await base.SendAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, AuthRequestFailedException.ErrorMessage);
            throw new AuthRequestFailedException(ex);
        }
    }
}
