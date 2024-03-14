using TelegramBot.Types;

namespace TelegramBot.Commands;

public class StartCommand : Command {
    public StartCommand(Bot bot): base(bot) {
        Names = new []{ "start" };
    }

    protected override string Run(Context context, CancellationToken cancellationToken)
    {
        Console.WriteLine("Called start.");
        return "Welcome! Press /help to see my functions.";
    }
}