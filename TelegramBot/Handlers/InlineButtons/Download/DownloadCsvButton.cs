using DataManager;
using Telegram.Bot;
using TelegramBot.Types;
using TelegramBot.Utils;
using Telegram.Bot.Types;
using TelegramBot.Utils.Logger;

namespace TelegramBot.Handlers.InlineButtons.Download;

public class DownloadCsvButton : Button
{
    public DownloadCsvButton()
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

        // Getting file in csv format.
        FormatProcessing formatProcessing = new();
        string path = UploadFilePath.Get(message);
        
        await using Stream outStream = formatProcessing.WriteToStream(path, FormatEnum.Csv);
        string fileName = Path.GetFileNameWithoutExtension(message.ReplyToMessage?.Document?.FileName) + ".csv";
        
        await context.BotClient.SendDocumentAsync(
            chatId: message.Chat.Id,
            document: new InputFileStream(outStream, fileName),
            caption: "Updated file in csv format",
            replyToMessageId: message.MessageId,
            cancellationToken: context.CancellationToken
        );
        
        Logger.Info(string.Format(
            "{0} upload file from path {1} in csv format completed.", 
            context.Update.CallbackQuery?.From, path));
    }
}