using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Messages
{
    public static partial class Messages
    {
        public static async Task SendMessageToAdminAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var chatId = update.Message.Chat.Id;
            var messageId = update.Message.MessageId;

            Users.UsersState[chatId] = UserStates.Other;

            await botClient.DeleteMessageAsync(
                chatId: chatId,
                messageId: messageId - 1);

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "main_menu")
                }

            });

            var text = "Ваша заявка отправлена. Если она будет рассмотрена, то вам придёт сообщение.";

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: text,
                replyMarkup: inlineKeyboard);

            var result = $"Одобрение заявки.\n\n{update.Message.Text}";

            InlineKeyboardMarkup inlineKeyboardtoAdmin = new(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Одобрить", callbackData: $"admin&1&{chatId}"),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Отклонить", callbackData: $"admin&0&{chatId}"),
                }
            });

            await botClient.SendTextMessageAsync(
            chatId: 6522857795,
            text: result,
            replyMarkup: inlineKeyboardtoAdmin);
        }
    }
}
