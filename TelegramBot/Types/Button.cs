using Telegram.Bot.Types.Enums;
using TelegramBot.Utils.Logger;

namespace TelegramBot.Types;

/// <summary>
/// Class for processing inline buttons.
/// </summary>
public class Button : Handler
{
    protected string Value = string.Empty;
    
    /// <summary>
    /// Validates inline buttons by update type and data in callback query.
    /// </summary>
    /// <param name="context">Update context.</param>
    /// <returns>If valid button.</returns>
    public override bool Validate(Context context)
    {
        return context.Update.Type == UpdateType.CallbackQuery 
               && Value.Equals(context.Update.CallbackQuery?.Data);
    }
    
    /// <summary>
    /// Base button handler.
    /// </summary>
    /// <param name="context">Update context.</param>
    public override async Task Handle(Context context)
    {
        Logger.Info($"{context.Update.CallbackQuery?.From} calls button {Value}.");
        await RunAsync(context);
    }

    /// <summary>
    /// Method for handling synchronous tasks.
    /// </summary>
    /// <param name="context">Update context.</param>
    /// <returns>Task.</returns>
    /// <exception cref="NotImplementedException">If method not implemented.</exception>
    protected virtual Task Run(Context context) 
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Method for handling asynchronous tasks.
    /// </summary>
    /// <param name="context">Update context.</param>
    protected virtual async Task RunAsync(Context context)
    {
        await Run(context);
    }
}