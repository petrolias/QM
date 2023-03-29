using System;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace QM.Core.Helper
{
    public class ScopedLogger<T> : ILogger<T>
    {
        private readonly ILogger<T> _logger;

        public ScopedLogger(ILogger<T> logger)
        {
            this._logger = logger;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return LogContext.PushProperty("Guid", Guid.NewGuid());
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return this._logger.IsEnabled(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            using (LogContext.PushProperty("Guid", Guid.NewGuid()))
            {
                _logger.Log(logLevel, eventId, state, exception, (s, ex) =>
                {
                    return $"[{LogContext.Properties["Guid"]}] {formatter(s, ex)}";
                });
            }
        }
    }
}
