using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Messages
{
    public static partial class Messages
    {
        public static async Task SendBecomeTutorMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;
            var messageId = update.CallbackQuery.Message.MessageId;

            Users.UsersState[chatId] = UserStates.BecomeTutor;

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "main_menu")
                }

            });

            await botClient.EditMessageTextAsync(
                    chatId: chatId,
                    messageId: messageId,
                    text: "Чтобы стать репетитором напишите сообщение, которое будет содержать:\n\nФИО: Иванов Иван Иванович\nУчебеное заведение: Школа\nПредмет: ОГЭ\nОписание:\nПример описания\n\ntg: @tg",
                    replyMarkup: inlineKeyboard,
                    cancellationToken: cancellationToken);
        }
    }
}
