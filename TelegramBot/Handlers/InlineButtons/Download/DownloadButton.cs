using TelegramBot.Types;
using TelegramBot.Utils;

namespace TelegramBot.Handlers.InlineButtons.Download;

/// <summary>
/// Button for providing download format type.
/// </summary>
public class DownloadButton : Button
{
    public DownloadButton()
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