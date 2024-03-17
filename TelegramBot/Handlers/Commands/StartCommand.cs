using TelegramBot.Types;

namespace TelegramBot.Handlers.Commands;

/// <summary>
/// First user command.
/// </summary>
public class StartCommand : Command {
    public StartCommand()
    {
        Name = "start";
    }

    /// <summary>
    /// <see cref="Command.Run"/>
    /// </summary>
    /// <returns>Welcome message for new users.</returns>
    protected override string Run(Context context)
    {
        return "Welcome! Attach file with data.";
    }
}