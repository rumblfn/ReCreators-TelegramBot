using TelegramBot.Types;
using TelegramBot.Utils;

namespace TelegramBot.Handlers.InlineButtons.Filter;

public class FilterButton : Button
{
    public FilterButton(Bot bot) : base(bot)
    {
        Value = "filter";
    }
    
    protected override async Task RunAsync(Context context)
    {
        await MessageUtils.EditTextFromCallbackAsync(
            context, 
            "Select filter field", 
            ReadyInlineKeyboardMarkups.FilterField);
    }
}