using DataManager;
using Telegram.Bot;
using TelegramBot.Types;
using TelegramBot.Utils;
using Telegram.Bot.Types;
using TelegramBot.Utils.Logger;

namespace TelegramBot.Handlers.InlineButtons.Filter;

/// <summary>
/// Filter button to provide fields available for filtering.
/// </summary>
public class FilterButton : Button
{
    public FilterButton()
    {
        Value = "filter";
    }

    /// <summary>
    /// Method for filtering data.
    /// </summary>
    /// <param name="context">Update context.</param>
    /// <param name="field">Filter field.</param>
    public static async Task HandleFilter(Context context, string field)
    {
        Message? reply = context.Update.Message?.ReplyToMessage;
        Message? message = context.Update.Message;
        
        // Check if message or reference message is empty.
        if (reply is null || message?.Text is null) return;
        
        // Getting data and check file format.
        FormatProcessing formatProc = new();
        
        string path = UploadFilePath.Get(reply);
        List<ReCreator>? reCreators = formatProc.ReadFile(path);

        if (reCreators == null || reCreators.Count < 2)
        {
            await context.BotClient.EditMessageTextAsync(
                chatId: reply.Chat.Id,
                messageId: reply.MessageId,
                text: "Empty list of data",
                cancellationToken: context.CancellationToken
            );
            return;
        }

        // Filter query.
        List<ReCreator> filteredReCreators = reCreators.Skip(1)
            .Where(reCreator =>
            {
                switch (field)
                {
                    case "MainObjects" when !reCreator.MainObjects.Contains(message.Text):
                    case "Workplace" when !reCreator.Workplace.Contains(message.Text):
                    case "RankYear" when !reCreator.RankYear.Contains(message.Text):
                        return false;
                    default:
                        return true;
                }
            })
            .ToList();

        reCreators = reCreators.Take(1).Concat(filteredReCreators).ToList();

        // Saving file.
        JsonProcessing jsonProc = new();
        Stream stream = jsonProc.Write(reCreators);
        FormatProcessing.SaveStreamToFile(stream, path);
        
        await context.BotClient.EditMessageTextAsync(
            chatId: reply.Chat.Id,
            messageId: reply.MessageId,
            text: $"Filter by field {field} and query {message.Text} completed. \n" +
                  $"Total objects count: {reCreators.Count - 1}",
            replyMarkup: ReadyInlineKeyboardMarkups.ActionType,
            cancellationToken: context.CancellationToken
        );
        
        Logger.Info(string.Format(
            "{0} filter data from path {1} by field: {2} and query: {3} completed.", 
            context.Update.Message?.From, path, field, message.Text));
    }
    
    protected override async Task RunAsync(Context context)
    {
        await MessageUtils.EditTextFromCallbackAsync(
            context, 
            "Select filter field", 
            ReadyInlineKeyboardMarkups.FilterField);
    }
}