using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Types;

public class Context
{
    public ITelegramBotClient BotClient { get; }
    public Update Update { get; }
    public CancellationToken CancellationToken { get; }
    
    public Context(Update update, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        Update = update;
        BotClient = botClient;
        CancellationToken = cancellationToken;
    }
}