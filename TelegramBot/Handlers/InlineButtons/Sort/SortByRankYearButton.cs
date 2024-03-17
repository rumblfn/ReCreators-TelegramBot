using DataManager;
using TelegramBot.Types;
using TelegramBot.Utils;
using Telegram.Bot.Types;
using TelegramBot.Utils.Logger;

namespace TelegramBot.Handlers.InlineButtons.Sort;

/// <summary>
/// Handler for sort by RankYear.
/// </summary>
public class SortByRankYearButton : Button
{
    public SortByRankYearButton()
    {
        Value = "sort:RankYear";
    }
    
    protected override async Task RunAsync(Context context)
    {
        Message? message = context.Update.CallbackQuery?.Message;
        if (message is null)
        {
            return;
        }
        
        FormatProcessing formatProc = new();
        
        string path = UploadFilePath.Get(message);
        List<ReCreator>? reCreators = formatProc.ReadFile(path);

        if (reCreators == null || reCreators.Count < 2)
        {
            await MessageUtils.EditTextFromCallbackAsync(
                context, 
                "Empty list of data",
                null);
            return;
        }

        List<ReCreator> sortedReCreators = reCreators.Skip(1)
            .OrderByDescending(reCreator => int.TryParse(
                reCreator.RankYear, out int parsedRankYear)
                ? parsedRankYear
                : 0)
            .ToList();
        
        reCreators = reCreators.Take(1).Concat(sortedReCreators).ToList();

        JsonProcessing jsonProc = new();
        Stream stream = jsonProc.Write(reCreators);
        FormatProcessing.SaveStreamToFile(stream, path);
        
        await MessageUtils.EditTextFromCallbackAsync(
            context, 
            "Sort by RankYear descending completed.",
            ReadyInlineKeyboardMarkups.ActionType);
        
        Logger.Info(string.Format(
            "{0} sort data from path {1} by field: \"RankYear\" and query: {2} completed.", 
            context.Update.CallbackQuery?.From, path, message.Text));
    }
}