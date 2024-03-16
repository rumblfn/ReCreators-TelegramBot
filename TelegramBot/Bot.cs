using TelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Handlers;
using TelegramBot.Handlers.Commands;
using TelegramBot.Handlers.InlineButtons.Download;
using TelegramBot.Handlers.InlineButtons.Exit;
using TelegramBot.Handlers.InlineButtons.Filter;
using TelegramBot.Handlers.InlineButtons.Sort;

namespace TelegramBot;

public class Bot {
    private List<Handler> Handlers { get; set; }

    private User? Client { get; set; }
    public string Token { get; init; }
    
    public Bot(string token)
    {
        Token = token;
        
        Handlers = new List<Handler> 
        {
            new FilterByWorkplaceButton(this),
            new FilterByMainObjectsButton(this),
            new FilterByRankYearButton(this),
            new SortByNameButton(this),
            new SortByRankYearButton(this),
            new SortButton(this),
            new ExitButton(this),
            new FilterButton(this),
            new DownloadButton(this),
            new DownloadCsvButton(this),
            new DownloadJsonButton(this),
            new StartCommand(this),
            new MessageHandler(this),
        };
    }
    
    public async Task Start() 
    {
        var botClient = new TelegramBotClient(Token);
        using var cts = new CancellationTokenSource();

        botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            cancellationToken: cts.Token
        );

        Client = await botClient.GetMeAsync(cancellationToken: cts.Token);
        Console.WriteLine($"Logged as @{Client.Username}");
        Console.Read();
        
        cts.Cancel();
    }

    private async Task HandleUpdateAsync(
        ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        ChatType? chatType = update.Message?.Chat.Type;
        if (chatType != null && chatType != ChatType.Private)
        {
            return;
        }

        var context = new Context(update, botClient, cancellationToken);
        Handler? handler = Handlers.Find(handler => handler.Validate(context));

        if (handler == null)
        {
            return;
        }
        
        try
        {
            await handler.Handle(context);
        }
        catch (Exception e)
        {
            Console.WriteLine("Something went wrong with processing handler.");
            Console.WriteLine($"Error: {e.Message}");
        }
    }
    
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
        return Task.CompletedTask;
    }
}