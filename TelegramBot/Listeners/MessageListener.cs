using TelegramBot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBot.Listeners;

public class MessageListener : Listener
{
    public MessageListener(Bot bot):base(bot) {}
    public override bool Validate(Context context, CancellationToken cancellationToken)
    {
        return context.Update.Type == UpdateType.Message;
    }
    public override async Task Handler(Context context, CancellationToken cancellationToken)
    {
        Console.WriteLine(context.Update.Message?.Text);
    }
}