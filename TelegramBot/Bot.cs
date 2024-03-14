using TelegramBot.Listeners;
using TelegramBot.Types;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using TelegramBot.Commands;

namespace TelegramBot;

public class Bot {
    private List<Command> Commands { get; set; }
    private List<Listener> Listeners { get; set; }
    
    public User? Client { get; private set; }
    public string? Token { get; init; }
    
    public Bot()
    {
        Listeners = new List<Listener> 
        {
            new MessageListener(this),
        };
        Commands = new List<Command>
        {
            new StartCommand(this),
            new HelpCommand(this),
            new MeCommand(this),
            new EchoCommand(this),
        };
    }
    
    public async Task Init() 
    {
        if (Token == null)
        {
            throw new Exception("Token not provided.");
        }
        
        var botClient = new TelegramBotClient(Token);
        using var cts = new CancellationTokenSource();

        // TODO: Add receiverOptions.
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
        var context = new Context(update, botClient);
        
        foreach (Listener listener in Listeners.Where(listener => listener.Validate(context, cancellationToken)))
        {
            await listener.Handler(context, cancellationToken);
        }
        foreach (Command command in Commands.Where(command => command.Validate(context, cancellationToken)))
        {
            await command.Handler(context, cancellationToken);
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