using TelegramBot.Types;

namespace TelegramBot.Commands;

public class MeCommand : Command {
    public MeCommand(Bot bot): base(bot) {
        Names = new []{ "me" };
    }

    protected override async Task<string> RunAsync(Context context, CancellationToken cancellationToken)
    {
        return "Me command";
    }
}