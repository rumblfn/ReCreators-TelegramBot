using DataManager;
using Telegram.Bot;
using TelegramBot.Utils;
using TelegramBot.Types;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Handlers.InlineButtons.Filter;
using TelegramBot.Utils.Logger;
using TelegramFile = Telegram.Bot.Types.File;

namespace TelegramBot.Handlers;

/// <summary>
/// Handler for processing message (documents and replies).
/// </summary>
public class MessageHandler : Handler
{
    /// <summary>
    /// Validates by update type.
    /// </summary>
    /// <param name="context">Update context.</param>
    /// <returns></returns>
    public override bool Validate(Context context)
    {
        return context.Update.Type == UpdateType.Message;
    }
    
    public override async Task Handle(Context context)
    {
        // Check for null message.
        Message? message = context.Update.Message;
        if (message is null)
        {
            return;
        }

        Logger.Info($"{context.Update.Message?.From} new message.");
        
        bool status = await HandleFilterQuery(context, message);
        await HandleFile(context, message, status);
    }

    /// <summary>
    /// Message file handler.
    /// </summary>
    /// <param name="context">Update context.</param>
    /// <param name="message">Context message.</param>
    /// <param name="isForFilter">If message for filter.</param>
    /// <exception cref="FormatException">If file format is broken.</exception>
    private static async Task HandleFile(Context context, Message message, bool isForFilter)
    {
        // Check for document.
        Document? document = message.Document;
        if (document is null)
        {
            if (!isForFilter)
            {
                await context.BotClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "You should attach file to work with bot.",
                    replyToMessageId: message.MessageId, 
                    cancellationToken: context.CancellationToken
                );
            }
            
            return;
        }

        // Check for file in document.
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
        
        // If file exist.
        Message reply = await context.BotClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Trying to download a file",
            replyToMessageId: message.MessageId,
            cancellationToken: context.CancellationToken
        );
        
        // Preparing stream to download file.
        string outputPath = UploadFilePath.Get(reply);
        FileStream createStream = new (outputPath, FileMode.Create);

        try
        {
            // Downloading file form message.
            await context.BotClient.DownloadFileAsync(file.FilePath, createStream, context.CancellationToken);
            
            // Processing file format.
            FormatProcessing fp = new();
            List<ReCreator>? reCreators = fp.ProcessFile(createStream, outputPath);

            if (reCreators == null)
            {
                throw new FormatException("The data format is broken");
            }

            // Everything is ok.
            await context.BotClient.EditMessageTextAsync(
                chatId: reply.Chat.Id,
                messageId: reply.MessageId,
                text: $"File <code>{document.FileName}</code> successfully downloaded. \n" +
                      "Choose action to work with data. \n" +
                      $"Parsed: <b>{reCreators.Count}</b> objects.",
                ParseMode.Html,
                replyMarkup: ReadyInlineKeyboardMarkups.ActionType,
                cancellationToken: context.CancellationToken);
            
            Logger.Info($"{context.Update.Message?.From} loaded new file {outputPath}.");
        }
        catch (Exception ex)
        {
            await context.BotClient.EditMessageTextAsync(
                reply.Chat.Id, reply.MessageId,
                $"Failed to download the file. \n" +
                $"Error: <code>{ex.Message}</code>",
                parseMode: ParseMode.Html,
                cancellationToken: context.CancellationToken);
            
            Logger.Error($"{context.Update.Message?.From} loading file failed {outputPath}.");
        }
        finally
        {
            createStream.Close();
        }
    }
    
    /// <summary>
    /// Check if message is reply for bot message and handle filter button.
    /// </summary>
    /// <param name="context">Update context.</param>
    /// <param name="message">User message.</param>
    private static async Task<bool> HandleFilterQuery(Context context, Message message)
    {
        if (message is not { Text: not null, ReplyToMessage.Text: not null })
        {
            return false;
        }
        
        string[] fieldsForFilter = { "MainObjects", "Workplace", "RankYear" };
        foreach (string field in fieldsForFilter)
        {
            // Check if reference message has filter content. 
            if (!message.ReplyToMessage.Text.Contains($"filter:{field}"))
            {
                continue;
            }
                
            await FilterButton.HandleFilter(context, field);
            return true;
        }

        return false;
    }
}