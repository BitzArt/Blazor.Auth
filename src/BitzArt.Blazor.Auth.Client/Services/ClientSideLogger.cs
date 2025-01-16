using Microsoft.Extensions.Logging;

namespace BitzArt.Blazor.Auth.Client;

internal class ClientSideLogger(ILoggerFactory factory) : ILogger
{
    private readonly ILogger _logger = factory.CreateLogger("Blazor.Auth.Client");

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        => _logger.BeginScope(state);

    public bool IsEnabled(LogLevel logLevel)
        => _logger.IsEnabled(logLevel);

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        => _logger.Log(logLevel, eventId, state, exception, formatter);
}
