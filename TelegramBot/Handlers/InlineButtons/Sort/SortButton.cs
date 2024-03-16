using TelegramBot.Types;
using TelegramBot.Utils;

namespace TelegramBot.Handlers.InlineButtons.Sort;

public class SortButton : Button
{
    public SortButton(Bot bot) : base(bot)
    {
        Value = "sort";
    }
    
    protected override async Task RunAsync(Context context)
    {
        await MessageUtils.EditTextFromCallbackAsync(
            context, 
            "Select sort type",
            ReadyInlineKeyboardMarkups.SortType);
    }
}