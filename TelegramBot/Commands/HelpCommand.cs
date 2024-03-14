using TelegramBot.Types;

namespace TelegramBot.Commands;

public class HelpCommand : Command {
    public HelpCommand(Bot bot): base(bot) {
        Names = new []{ "help" };
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        return "Help command";
    }
}