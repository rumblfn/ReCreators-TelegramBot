namespace TelegramBot.Types;

public abstract class Handler
{
    protected Bot Bot { get; }

    protected Handler(Bot bot)
    {
        Bot = bot;
    }
    
    public abstract bool Validate(Context context);
    public abstract Task Handle(Context context);
}