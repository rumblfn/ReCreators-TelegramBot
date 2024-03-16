using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Utils;

public static class ReadyInlineKeyboardMarkups
{
    public static readonly InlineKeyboardMarkup FileType = new(new[]
    {
        new []
        {
            InlineKeyboardButton.WithCallbackData("CSV", "csv"),
            InlineKeyboardButton.WithCallbackData("JSON", "json"),
        },
        new []
        {
            InlineKeyboardButton.WithCallbackData("Exit", "exit")
        }
    });
    
    public static readonly InlineKeyboardMarkup ActionType = new(new[]
    {
        new []
        {
            InlineKeyboardButton.WithCallbackData("Filter", "filter"),
            InlineKeyboardButton.WithCallbackData("Sort", "sort"),
        },
        new []
        {
            InlineKeyboardButton.WithCallbackData("Download", "download"),
        },
    });
    
    public static readonly InlineKeyboardMarkup FilterField = new(new[]
    {
        new []
        {
            InlineKeyboardButton.WithCallbackData("MainObjects", "MainObjects"),
            InlineKeyboardButton.WithCallbackData("Workplace", "Workplace"),
            InlineKeyboardButton.WithCallbackData("RankYear", "RankYear"),
        },
        new []
        {
            InlineKeyboardButton.WithCallbackData("Exit", "exit")
        }
    });
    
    public static readonly InlineKeyboardMarkup SortType = new(new[]
    {
        new []
        {
            InlineKeyboardButton.WithCallbackData("Name alphabetical", "Name"),
            InlineKeyboardButton.WithCallbackData("RankYear descending", "RankYear")
        },
        new []
        {
            InlineKeyboardButton.WithCallbackData("Exit", "exit")
        }
    });
    
    public static readonly InlineKeyboardMarkup Exit = new(new[]
    {
        new []
        {
            InlineKeyboardButton.WithCallbackData("Exit", "exit")
        }
    });
}