using DataManager;
using Telegram.Bot;
using TelegramBot.Types;
using TelegramBot.Utils;
using Telegram.Bot.Types;

namespace TelegramBot.Handlers.InlineButtons.Filter;

public class FilterButton : Button
{
    public FilterButton(Bot bot) : base(bot)
    {
        Value = "filter";
    }

    public static async Task HandleFilter(Context context, string field)
    {
        Message? reply = context.Update.Message?.ReplyToMessage;
        Message? message = context.Update.Message;
        
        if (reply is null || message?.Text is null)
        {
            await MessageUtils.EditTextFromCallbackAsync(
                context, 
                "Message or reply is empty.",
                null);
            return;
        }

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

        List<ReCreator> filteredReCreators = reCreators
            .Select((reCreator, index) => new { Index = index, reCreator })
            .Where(item =>
            {
                switch (field)
                {
                    case "MainObjects" when !item.reCreator.MainObjects.Contains(message.Text):
                    case "Workplace" when !item.reCreator.Workplace.Contains(message.Text):
                    case "RankYear" when !item.reCreator.RankYear.Contains(message.Text):
                        return false;
                    default:
                        return item.Index > 1;
                }
            })
            .Select(item => item.reCreator)
            .ToList();

        reCreators = reCreators.Take(1).Concat(filteredReCreators).ToList();

        JsonProcessing jsonProc = new();
        Stream stream = jsonProc.Write(reCreators);
        formatProc.SaveStreamToFile(stream, path);
        
        await context.BotClient.EditMessageTextAsync(
            chatId: reply.Chat.Id,
            messageId: reply.MessageId,
            text: $"Filter by field {field} and query {message.Text} completed. \n" +
                  $"Total objects count: {reCreators.Count}",
            replyMarkup: ReadyInlineKeyboardMarkups.ActionType,
            cancellationToken: context.CancellationToken
        );
    }
    
    protected override async Task RunAsync(Context context)
    {
        await MessageUtils.EditTextFromCallbackAsync(
            context, 
            "Select filter field", 
            ReadyInlineKeyboardMarkups.FilterField);
    }
}