using TelegramBot.Types;
using TelegramBot.Utils;

namespace TelegramBot.Handlers.InlineButtons.Filter;

public class FilterByWorkplaceButton : Button
{
    public FilterByWorkplaceButton()
    {
        Value = "filter:Workplace";
    }
    
    protected override async Task RunAsync(Context context)
    {
        await MessageUtils.EditTextFromCallbackAsync(
            context, 
            $"Filter by: {Value}. \n" +
                "Reply to this message with your filter query", 
            ReadyInlineKeyboardMarkups.Exit);
    }
}