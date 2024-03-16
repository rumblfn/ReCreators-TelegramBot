using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Types;

namespace TelegramBot.Utils;

public static class MessageUtils
{
    public static async Task EditTextFromCallbackAsync(
        Context context, string text, InlineKeyboardMarkup? inlineKeyboardMarkup)
    {
        Message? message = context.Update.CallbackQuery?.Message;
        if (message is null)
        {
            return;
        }

        await context.BotClient.EditMessageTextAsync(
            chatId: message.Chat.Id,
            messageId: message.MessageId,
            text: text, 
            replyMarkup: inlineKeyboardMarkup,
            cancellationToken: context.CancellationToken
        );
    }
}