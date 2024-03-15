using TelegramBot.Types;
using TelegramBot.Utils;

namespace TelegramBot.Handlers.InlineButtons.Exit;

public class ExitButton : Button
{
    public ExitButton(Bot bot) : base(bot)
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