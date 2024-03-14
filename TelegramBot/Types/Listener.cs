namespace TelegramBot.Types;

public abstract class Listener
{
    protected Bot Bot { get; }

    protected Listener(Bot bot)
    {
        Bot = bot;
    }
    
    public abstract bool Validate(Context context, CancellationToken cancellationToken);
    public abstract Task Handler(Context context, CancellationToken cancellationToken);
}