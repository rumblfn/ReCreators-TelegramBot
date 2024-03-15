using TelegramBot.Types;

namespace TelegramBot.Handlers.Commands;

public class StartCommand : Command {
    public StartCommand(Bot bot): base(bot)
    {
        Name = "start";
    }

    protected override string Run(Context context)
    {
        return "Welcome! Attach file with data.";
    }
}