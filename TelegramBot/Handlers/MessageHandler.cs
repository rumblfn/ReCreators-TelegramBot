using DataManager;
using Telegram.Bot;
using TelegramBot.Utils;
using TelegramBot.Types;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramFile = Telegram.Bot.Types.File;

namespace TelegramBot.Handlers;

public class MessageHandler : Handler
{
    public MessageHandler(Bot bot):base(bot) {}
    public override bool Validate(Context context)
    {
        return context.Update.Type == UpdateType.Message;
    }
    public override async Task Handle(Context context)
    {
        Message? message = context.Update.Message;
        if (message is null)
        {
            return;
        }
        
        Document? document = message.Document;
        if (document is null)
        {
            await context.BotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "You should attach file to work with bot.",
                replyToMessageId: message.MessageId, 
                cancellationToken: context.CancellationToken
            );
            return;
        }

        // TODO: Check file format.
        TelegramFile file = await context.BotClient.GetFileAsync(document.FileId, context.CancellationToken);
        if (file.FilePath == null)
        {
            await context.BotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Empty file path.",
                replyToMessageId: message.MessageId, 
                cancellationToken: context.CancellationToken
            );
            return;
        }
        
        Message reply = await context.BotClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Trying to download a file",
            replyToMessageId: message.MessageId,
            cancellationToken: context.CancellationToken
        );
        
        string outputPath = UploadFilePath.Get(reply);
        await using var fs = new FileStream(outputPath, FileMode.Create);

        try
        {
            await context.BotClient.DownloadFileAsync(file.FilePath, fs, context.CancellationToken);

            FormatProcessing fp = new();
            List<ReCreator>? reCreators = fp.ProcessFile(fs, outputPath);

            if (reCreators == null)
            {
                throw new FormatException("The data format is broken");
            }
            
            await context.BotClient.EditMessageTextAsync(
                chatId: reply.Chat.Id, 
                messageId: reply.MessageId, 
                text: $"File <code>{document.FileName}</code> successfully downloaded. \n" +
                      "Choose action to work with data. \n" +
                      $"Parsed: <b>{reCreators.Count}</b> objects.",
                ParseMode.Html,
                replyMarkup: ReadyInlineKeyboardMarkups.ActionType,
                cancellationToken: context.CancellationToken);
        }
        catch (Exception ex)
        {
            await context.BotClient.EditMessageTextAsync(
                reply.Chat.Id, reply.MessageId, 
                $"Failed to download the file. \n" +
                    $"Error: <code>{ex.Message}</code>",
                parseMode: ParseMode.Html,
                cancellationToken: context.CancellationToken);
        }
        finally
        {
            fs.Close();
            await fs.DisposeAsync();
        }
    }
}