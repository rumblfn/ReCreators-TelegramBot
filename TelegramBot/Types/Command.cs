using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace TelegramBot.Types;

public abstract class Command : Listener
{
    protected string[] Names { get; init; } = Array.Empty<string>();

    protected Command(Bot bot) : base(bot) 
    {
        
    }
    
    public override bool Validate(Context context, CancellationToken cancellationToken)
    {
        if (context.Update.Type != UpdateType.Message ||
            context.Update.Message!.Type != MessageType.Text ||
            context.Update.Message.Text == null)
        {
            return false;
        }
        
        string messageText = context.Update.Message.Text.Replace($"@{Bot.Client.Username}","");
        return Names.Any(name => messageText.StartsWith($"{name} ") || messageText.Equals(name));
    }
    
    public override async Task Handler(Context context, CancellationToken cancellationToken)
    {
        string response = await RunAsync(context, cancellationToken);
        
        if (context.Update.Message != null)
        {
            long chatId = context.Update.Message.Chat.Id;
            
            if (response.Length == 0)
            {
                return;
            }
            
            await context.BotClient.SendTextMessageAsync(
                chatId: chatId,
                text: response,
                parseMode: ParseMode.MarkdownV2,
                replyToMessageId: context.Update.Message.MessageId, 
                cancellationToken: cancellationToken
            );
        }
    }

    protected virtual string Run(Context context, CancellationToken cancellationToken) 
    {
        throw new NotImplementedException();
    }
    
    protected virtual async Task<string> RunAsync(Context context, CancellationToken cancellationToken)
    {
        return Run(context, cancellationToken);
    }
}