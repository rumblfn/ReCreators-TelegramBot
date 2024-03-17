using DataManager;
using TelegramBot.Types;
using TelegramBot.Utils;
using Telegram.Bot.Types;
using TelegramBot.Utils.Logger;

namespace TelegramBot.Handlers.InlineButtons.Sort;

/// <summary>
/// Handler for sort by Name.
/// </summary>
public class SortByNameButton : Button
{
    public SortByNameButton()
    {
        Value = "sort:Name";
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
            .OrderBy(reCreator => reCreator.Name)
            .ToList();

        reCreators = reCreators.Take(1).Concat(sortedReCreators).ToList();

        JsonProcessing jsonProc = new();
        Stream stream = jsonProc.Write(reCreators);
        FormatProcessing.SaveStreamToFile(stream, path);
        
        await MessageUtils.EditTextFromCallbackAsync(
            context, 
            "Sort by name alphabetical completed.",
            ReadyInlineKeyboardMarkups.ActionType);
        
        Logger.Info(string.Format(
            "{0} sort data from path {1} by field: \"Name\" and query: {2} completed.", 
            context.Update.CallbackQuery?.From, path, message.Text));
    }
}