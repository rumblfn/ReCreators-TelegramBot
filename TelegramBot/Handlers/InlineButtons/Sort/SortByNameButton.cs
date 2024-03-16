using DataManager;
using TelegramBot.Types;
using TelegramBot.Utils;
using Telegram.Bot.Types;

namespace TelegramBot.Handlers.InlineButtons.Sort;

public class SortByNameButton : Button
{
    public SortByNameButton(Bot bot) : base(bot)
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

        List<ReCreator> sortedReCreators = reCreators
            .Select((reCreator, index) => new { Index = index, reCreator })
            .Where(item => item.Index > 1)
            .OrderBy(item => item.reCreator.Name)
            .Select(item => item.reCreator)
            .ToList();

        reCreators = reCreators.Take(1).Concat(sortedReCreators).ToList();

        JsonProcessing jsonProc = new();
        Stream stream = jsonProc.Write(reCreators);
        formatProc.SaveStreamToFile(stream, path);
        
        await MessageUtils.EditTextFromCallbackAsync(
            context, 
            "Sort by name alphabetical completed.",
            ReadyInlineKeyboardMarkups.ActionType);
    }
}