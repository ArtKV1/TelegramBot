using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Messages.BecomeTutor
{
    public static partial class Messages
    {
        public static async Task SendFirstMessageBecomeTutor(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;
            var messageId = update.CallbackQuery.Message.MessageId;

            InlineKeyboardMarkup inlineKeyboard = new(new[]
                        {
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Школа", callbackData: "become_tutor_school"),
                    InlineKeyboardButton.WithCallbackData(text: "Университет", callbackData: "become_tutor_university"),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "main_menu")
                }

            });

            await botClient.EditMessageTextAsync(
                    chatId: chatId,
                    messageId: messageId,
                    text: "Для продолжения выберите учебное заведение.",
                    replyMarkup: inlineKeyboard,
                    cancellationToken: cancellationToken);
        }
    }
}
