using DataManager;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Types;
using TelegramBot.Utils;
using SystemFile = System.IO.File;

namespace TelegramBot.Handlers.InlineButtons.Download;

public class DownloadJsonButton : Button
{
    public DownloadJsonButton(Bot bot) : base(bot)
    {
        Value = "json";
    }
        
    protected override async Task RunAsync(Context context)
    {
        Message? message = context.Update.CallbackQuery?.Message;
        if (message is null)
        {
            return;
        }
        
        FormatProcessing formatProcessing = new();
        string path = UploadFilePath.Get(message);
        
        await using Stream outStream = formatProcessing.WriteToStream(path, FormatEnum.Json);
        string fileName = Path.GetFileNameWithoutExtension(message.ReplyToMessage?.Document?.FileName) + ".json";

        await context.BotClient.SendDocumentAsync(
            chatId: message.Chat.Id,
            document: new InputFileStream(outStream, fileName),
            caption: "Updated file in json format",
            replyToMessageId: message.MessageId,
            cancellationToken: context.CancellationToken
        );
    }
}