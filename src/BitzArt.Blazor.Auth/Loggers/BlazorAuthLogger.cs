using Microsoft.Extensions.Logging;

namespace BitzArt.Blazor.Auth;

internal class BlazorAuthLogger : IBlazorAuthLogger
{
    private protected virtual string CategoryName => "Blazor.Auth";

    public BlazorAuthLogger(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger(CategoryName);
    }

    private readonly ILogger _logger;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        => _logger.BeginScope(state);

    public bool IsEnabled(LogLevel logLevel)
        => _logger.IsEnabled(logLevel);

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        => _logger.Log(logLevel, eventId, state, exception, formatter);
}
