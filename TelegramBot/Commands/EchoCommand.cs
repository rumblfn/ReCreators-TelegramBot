using Telegram.Bot.Types;
using TelegramBot.Types;

namespace TelegramBot.Commands;

public class EchoCommand : Command {
    public EchoCommand(Bot bot): base(bot) {
        Names = new []{ "echo" };
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        Message? message = context.Update.Message;
        return $"Echo: {message}";
    }
}