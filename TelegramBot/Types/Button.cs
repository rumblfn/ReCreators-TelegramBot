using Telegram.Bot.Types.Enums;

namespace TelegramBot.Types;

public class Button : Handler
{
    protected string Value = string.Empty;
    
    protected Button(Bot bot) : base(bot) 
    {
        
    }
    
    public override bool Validate(Context context)
    {
        return context.Update.Type == UpdateType.CallbackQuery 
               && Value.Equals(context.Update.CallbackQuery?.Data);
    }
    
    public override async Task Handle(Context context)
    {
        await RunAsync(context);
    }

    protected virtual Task Run(Context context) 
    {
        throw new NotImplementedException();
    }
    
    protected virtual async Task RunAsync(Context context)
    {
        await Run(context);
    }
}