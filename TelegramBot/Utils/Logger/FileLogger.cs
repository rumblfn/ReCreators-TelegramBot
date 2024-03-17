using System.Text;
using Microsoft.Extensions.Logging;

namespace TelegramBot.Utils.Logger;

/// <summary>
/// File logger for logging to file.
/// Necessary for <see cref="LoggerFactory"/>
/// </summary>
public class FileLogger : ILogger
{
    private readonly string _path;
    private static readonly object Lock = new ();

    public FileLogger(string path)
    {
        _path = path;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        // Check if log level enabled.
        if (!IsEnabled(logLevel))
        {
            return;
        }
        
        string message = $"{logLevel.ToString()}: {DateTime.Now} - {formatter(state, exception)}";
        // Lock to avoid collisions.
        lock (Lock)
        {
            File.AppendAllText(_path, message + Environment.NewLine, Encoding.UTF8);
        }
    }

    public bool IsEnabled(LogLevel logLevel) => true;
    
    public IDisposable BeginScope<TState>(TState state) where TState : notnull
    {
        throw new NotImplementedException();
    }
}