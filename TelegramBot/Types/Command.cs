using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace TelegramBot.Types;

public abstract class Command : Handler
{
    private readonly string _name = string.Empty;
    protected string Name
    {
        get => _name;
        init => _name = "/" + value;
    }

    protected Command(Bot bot) : base(bot) 
    {
        
    }
    
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
        string response = await RunAsync(context);
        
        if (context.Update.Message != null)
        {
            long chatId = context.Update.Message.Chat.Id;
            
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

    protected virtual string Run(Context context) 
    {
        throw new NotImplementedException();
    }
    
    protected virtual Task<string> RunAsync(Context context)
    {
        return Task.FromResult(Run(context));
    }
}