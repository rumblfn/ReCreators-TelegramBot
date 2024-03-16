using Telegram.Bot.Types;

namespace TelegramBot.Utils;

public static class UploadFilePath
{
    public static string Get(Message message, string extension = "json")
    {
        const string tail = "../upload";
        if (!Directory.Exists(tail))
        {
            Directory.CreateDirectory(tail);
        }
        
        return $"{tail}/{message.Chat.Id}-{message.MessageId}.{extension}";
    }
}