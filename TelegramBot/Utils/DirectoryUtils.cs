namespace TelegramBot.Utils;

/// <summary>
/// Utils for handling directories.
/// </summary>
public static class DirectoryUtils
{
    /// <summary>
    /// Searching for directory with .sln file (Solution directory).
    /// Uses recursive descent.
    /// </summary>
    /// <returns>Solution directory.</returns>
    public static DirectoryInfo? GetSolutionDir()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (directory != null && !directory.GetFiles("*.sln").Any())
        {
            directory = directory.Parent;
        }
        
        return directory;
    }
}