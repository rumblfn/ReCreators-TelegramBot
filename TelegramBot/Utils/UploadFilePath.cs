using Telegram.Bot.Types;

namespace TelegramBot.Utils;

/// <summary>
/// Utils for upload files path.
/// </summary>
public static class UploadFilePath
{
    /// <summary>
    /// Returns prepared path for new uploaded file.
    /// If upload directory not found creates it.
    /// </summary>
    /// <param name="message">Telegram message.</param>
    /// <param name="extension">File extension.</param>
    /// <returns>Path to file that associated with message.</returns>
    public static string Get(Message message, string extension = "json")
    {
        DirectoryInfo? solutionPath = DirectoryUtils.GetSolutionDir();
        string uploadDirPath = solutionPath + "/upload";

        // Create upload directory if not exist.
        if (!Directory.Exists(uploadDirPath))
        {
            Directory.CreateDirectory(uploadDirPath);
        }
        
        return $"{uploadDirPath}/{message.Chat.Id}-{message.MessageId}.{extension}";
    }
}