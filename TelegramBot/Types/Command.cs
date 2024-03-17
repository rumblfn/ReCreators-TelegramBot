using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TelegramBot.Utils.Logger;

namespace TelegramBot.Types;

/// <summary>
/// Command handler.
/// Synchronous update processing.
/// </summary>
public abstract class Command : Handler
{
    private readonly string _name = string.Empty;
    protected string Name
    {
        get => _name;
        init => _name = "/" + value;
    }
    
    /// <summary>
    /// Validate update with specified Name in command.
    /// </summary>
    /// <param name="context">Update context.</param>
    /// <returns>Is valid.</returns>
    public override bool Validate(Context context)
    {
        if (context.Update.Type != UpdateType.Message ||
            context.Update.Message!.Type != MessageType.Text ||
            context.Update.Message.Text == null)
        {
            return false;
        }

        string messageText = context.Update.Message.Text;
        return messageText.Split(' ')[0].Equals(Name);
    }
    
    public override async Task Handle(Context context)
    {
        Logger.Info($"{context.Update.Message?.From} calls command {Name}.");
        
        // Get command text.
        string response = Run(context);
        
        if (context.Update.Message != null)
        {
            long chatId = context.Update.Message.Chat.Id;
            
            // Processing command response.
            if (response.Length == 0)
            {
                response = "Empty response, try another way.";
            }
            
            await context.BotClient.SendTextMessageAsync(
                chatId: chatId,
                text: response,
                replyToMessageId: context.Update.Message.MessageId, 
                cancellationToken: context.CancellationToken
            );
        }
    }

    /// <summary>
    /// Update processing.
    /// Only plain text.
    /// </summary>
    /// <param name="context">Update context.</param>
    /// <returns>Command text.</returns>
    /// <exception cref="NotImplementedException">If command Run method not implemented.</exception>
    protected virtual string Run(Context context) 
    {
        throw new NotImplementedException();
    }
}