using Telegram.Bot;
using TelegramBot.Types;
using Telegram.Bot.Types;
using TelegramBot.Handlers;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using TelegramBot.Utils.Logger;
using TelegramBot.Handlers.Commands;
using TelegramBot.Handlers.InlineButtons.Exit;
using TelegramBot.Handlers.InlineButtons.Sort;
using TelegramBot.Handlers.InlineButtons.Filter;
using TelegramBot.Handlers.InlineButtons.Download;

namespace TelegramBot;

/// <summary>
/// Telegram Bot wrapper to creating instance and processing updates.
/// </summary>
public class Bot {
    private List<Handler> Handlers { get; }
    private string Token { get; }
    
    /// <summary>
    /// Constructor for saving token and initialize update handlers.
    /// </summary>
    /// <param name="token">Telegram bot token.</param>
    public Bot(string token)
    {
        Token = token;
        
        Handlers = new List<Handler> 
        {
            new FilterButton(),
            new FilterByRankYearButton(),
            new FilterByWorkplaceButton(),
            new FilterByMainObjectsButton(),
            
            new SortButton(),
            new SortByNameButton(),
            new SortByRankYearButton(),
            
            new DownloadButton(),
            new DownloadCsvButton(),
            new DownloadJsonButton(),
            
            new ExitButton(),
            new StartCommand(),
            new MessageHandler(),
        };
    }
    
    /// <summary>
    /// Creates instance of the bot and starts receiving updates.
    /// </summary>
    public async Task Start() 
    {
        // Creating instance of telegram bot client.
        var botClient = new TelegramBotClient(Token);
        using var cts = new CancellationTokenSource();

        // Start updates receiver.
        botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            cancellationToken: cts.Token
        );
        
        // Testing instance.
        User client = await botClient.GetMeAsync(cts.Token);

        // Notification that the bot is running.
        string message = $"Logged as @{client.Username}";
        Logger.Info(message);
        Console.WriteLine(message);
        
        // To avoid shutting down the bot immediately after launch.
        Console.Read();
        
        cts.Cancel();
    }

    /// <summary>
    /// Method for handling updates.
    /// </summary>
    /// <param name="botClient">Instance of the bot.</param>
    /// <param name="update">Update event.</param>
    /// <param name="cancellationToken">Token to cancel update event.</param>
    private async Task HandleUpdateAsync(
        ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        // Check if update from private chat.
        ChatType? chatType = update.Message?.Chat.Type;
        if (chatType != null && chatType != ChatType.Private) return;
        
        // Creating update event data instance.
        var context = new Context(update, botClient, cancellationToken);
        
        // Check if any handler can process update.
        Handler? handler = Handlers.Find(handler => handler.Validate(context));
        if (handler == null) return;
        
        try
        {
            await handler.Handle(context);
        }
        catch (Exception e)
        {
            Console.WriteLine("Something went wrong with processing handler.");
            Logger.Error(e.Message);
        }
    }
    
    /// <summary>
    /// Method for handling Telegram api errors from receiver.
    /// </summary>
    /// <param name="botClient">Instance of the bot.</param>
    /// <param name="exception">Exception to handle.</param>
    /// <param name="cancellationToken">Token to cancel event.</param>
    /// <returns>Finished Task.</returns>
    private static Task HandleErrorAsync(
        ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        string errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };
        
        Console.WriteLine(errorMessage);
        Logger.Error(errorMessage);
        
        return Task.CompletedTask;
    }
}