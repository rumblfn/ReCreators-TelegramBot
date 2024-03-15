using Telegram.Bot.Types;

namespace TelegramBot.Utils;

public static class UploadFilePath
{
    public static string Get(Message message)
    {
        return $"upload/{message.Chat.Id}-{message.MessageId}.json";
    }
}