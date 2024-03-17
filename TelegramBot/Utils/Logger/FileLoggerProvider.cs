using Microsoft.Extensions.Logging;

namespace TelegramBot.Utils.Logger;

/// <summary>
/// Simple logger provider for logging to file.
/// Necessary for <see cref="LoggerFactory"/>
/// </summary>
public class FileLoggerProvider : ILoggerProvider
{
    private readonly string _path;

    public FileLoggerProvider(string path)
    {
        _path = path;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new FileLogger(_path);
    }

    public void Dispose()
    {
        
    }
}