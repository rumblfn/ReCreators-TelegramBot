using TelegramBot.Types;
using TelegramBot.Utils;

namespace TelegramBot.Handlers.InlineButtons.Exit;

/// <summary>
/// Exit button for providing main panel.
/// </summary>
public class ExitButton : Button
{
    public ExitButton()
    {
        Value = "exit";
    }
        
    protected override async Task RunAsync(Context context)
    {
        await MessageUtils.EditTextFromCallbackAsync(
            context, 
            "Choose action to work with data.", 
            ReadyInlineKeyboardMarkups.ActionType);
    }
}