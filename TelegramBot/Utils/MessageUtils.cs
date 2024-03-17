using Telegram.Bot;
using TelegramBot.Types;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Utils;

/// <summary>
/// Utils for working with messages.
/// </summary>
public static class MessageUtils
{
    /// <summary>
    /// Method to edit message that triggered by inline button.
    /// Check if callback message not null.
    /// </summary>
    /// <param name="context">Update context.</param>
    /// <param name="text">New message text.</param>
    /// <param name="inlineKeyboardMarkup">New message <see cref="InlineKeyboardMarkup"/></param>
    public static async Task EditTextFromCallbackAsync(
        Context context, string text, InlineKeyboardMarkup? inlineKeyboardMarkup)
    {
        // Check for null message.
        Message? message = context.Update.CallbackQuery?.Message;
        if (message is null)
        {
            return;
        }

        // Update message from callback query.
        await context.BotClient.EditMessageTextAsync(
            chatId: message.Chat.Id,
            messageId: message.MessageId,
            text: text, 
            replyMarkup: inlineKeyboardMarkup,
            cancellationToken: context.CancellationToken
        );
    }
}