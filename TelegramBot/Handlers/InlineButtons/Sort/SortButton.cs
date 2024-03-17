using TelegramBot.Types;
using TelegramBot.Utils;

namespace TelegramBot.Handlers.InlineButtons.Sort;

/// <summary>
/// Sort button to provide fields available for sorting.
/// </summary>
public class SortButton : Button
{
    public SortButton()
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