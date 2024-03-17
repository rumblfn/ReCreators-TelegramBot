using Microsoft.Extensions.Logging;

namespace TelegramBot.Utils.Logger;

/// <summary>
/// Provides static methods for logging information messages and errors.
/// </summary>
public static class Logger
{
    private const string LogsDirectoryPath = "/var";
    
    private const string LogPath = "/log.txt";
    private const string ErrorPath = "/error.txt";

    /// <summary>
    /// Creates an instance of <see cref="ILogger"/> for logging to a file located at the specified path.
    /// In case of an error, it creates a logger that outputs logs to the console.
    /// </summary>
    /// <param name="path">The path to the log file.</param>
    /// <returns>An instance of <see cref="ILogger"/>.</returns>
    private static ILogger GetLogger(string path)
    {
        try
        {
            DirectoryInfo? solutionPath = DirectoryUtils.GetSolutionDir();
            
            // Create var directory if not exist.
            string varPath = solutionPath + LogsDirectoryPath;
            if (!Directory.Exists(varPath))
            {
                Directory.CreateDirectory(varPath);
            }
            
            using ILoggerFactory factory = LoggerFactory.Create(builder => 
            {
                builder.AddProvider(new FileLoggerProvider(varPath + path));
            });
            return factory.CreateLogger("Program");
        }
        catch (Exception)
        {
            using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
            return factory.CreateLogger("Program");
        }
    }

    /// <summary>
    /// Logs information message.
    /// </summary>
    /// <param name="message">Message for logging.</param>
    public static void Info(string message)
    {
        ILogger logger = GetLogger(LogPath);

        try
        {
            logger.LogInformation("Bot: {Description}", message);
        }
        catch (Exception ex)
        {
            // Output message and error to console if logging failed.
            Console.WriteLine(message);
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Logs error message.
    /// </summary>
    /// <param name="message">Message for logging.</param>
    public static void Error(string message)
    {
        ILogger logger = GetLogger(ErrorPath);

        try
        {
            logger.LogError("Bot: {Description}", message);
        }
        catch (Exception ex)
        {
            // Output message and error to console if logging failed.
            Console.WriteLine(message);
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}