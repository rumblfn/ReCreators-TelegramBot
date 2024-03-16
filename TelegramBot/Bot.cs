using Telegram.Bot;
using TelegramBot.Types;
using Telegram.Bot.Types;
using TelegramBot.Handlers;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using TelegramBot.Handlers.Commands;
using TelegramBot.Handlers.InlineButtons.Exit;
using TelegramBot.Handlers.InlineButtons.Sort;
using TelegramBot.Handlers.InlineButtons.Filter;
using TelegramBot.Handlers.InlineButtons.Download;

namespace TelegramBot;

public class Bot {
    private List<Handler> Handlers { get; }

    private User? Client { get; set; }
    private string Token { get; }
    
    public Bot(string token)
    {
        Token = token;
        
        Handlers = new List<Handler> 
        {
            new FilterButton(this),
            new FilterByRankYearButton(this),
            new FilterByWorkplaceButton(this),
            new FilterByMainObjectsButton(this),
            new SortButton(this),
            new SortByNameButton(this),
            new SortByRankYearButton(this),
            new DownloadButton(this),
            new DownloadCsvButton(this),
            new DownloadJsonButton(this),
            new ExitButton(this),
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