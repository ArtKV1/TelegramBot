using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Messages.BecomeTutor
{
    public static partial class Messages
    {
        public static async Task SendMessageToAdminAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;
            var messageId = update.CallbackQuery.Message.MessageId;

            Users.UsersState[chatId] = UserStates.Other;

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "main_menu")
                }

            });

            var text = "Ваша заявка отправлена. Если она будет рассмотрена, то вам придёт сообщение.";

            await botClient.EditMessageTextAsync(
                chatId: chatId,
                messageId: messageId,
                text: text,
                replyMarkup: inlineKeyboard);

            var result = $"Одобрение заявки.\n\n{update.CallbackQuery.Message.Text.Remove(0, 39)}";

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
