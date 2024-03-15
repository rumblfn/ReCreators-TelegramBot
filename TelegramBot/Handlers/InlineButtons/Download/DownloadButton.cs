using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Types;
using TelegramBot.Utils;

namespace TelegramBot.Handlers.InlineButtons.Download;

public class DownloadButton : Button
{
    public DownloadButton(Bot bot) : base(bot)
    {
        Value = "download";
    }
        
    protected override async Task RunAsync(Context context)
    {
        await MessageUtils.EditTextFromCallbackAsync(
            context, 
            "Select the file format you want to download", 
            ReadyInlineKeyboardMarkups.FileType);
    }
}