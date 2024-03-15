using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Types;
using TelegramBot.Utils;
using SystemFile = System.IO.File;

namespace TelegramBot.Handlers.InlineButtons.Download;

public class DownloadCsvButton : Button
{
    public DownloadCsvButton(Bot bot) : base(bot)
    {
        Value = "csv";
    }
        
    protected override async Task RunAsync(Context context)
    {
        Message? message = context.Update.CallbackQuery?.Message;
        if (message is null)
        {
            return;
        }

        await using FileStream sendFileStream = SystemFile.Open(UploadFilePath.Get(message), FileMode.Open);
        await context.BotClient.SendDocumentAsync(
            chatId: message.Chat.Id,
            document: new InputFileStream(sendFileStream, message.ReplyToMessage?.Document?.FileName ?? "File.csv"),
            caption: "Updated file",
            replyToMessageId: message.MessageId,
            cancellationToken: context.CancellationToken
        );
    }
}