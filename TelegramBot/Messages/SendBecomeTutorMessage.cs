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

            Users.LastMessage[chatId] = messageId;

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "main_menu")
                }

            });

            var text = "Чтобы стать репетитором, перешлите это сообщение и добавьте к нему ваши данные:\n\nФИО: Иванов Иван Иванович\nУчебеное заведение: Школа\nПредмет: Русский язык, ОГЭ(вторая часть)\nОписание:\nПример описания\n\ntg: @tg";

            if (update.CallbackQuery.Message.Text.Contains("Проверьте все данные перед отправкой."))
            {
                text += "\n\nДанные которые вы заполнили до этого:\n\n" + update.CallbackQuery.Message.Text.Remove(0, 39);
            }

            await botClient.EditMessageTextAsync(
                    chatId: chatId,
                    messageId: messageId,
                    text: text,
                    replyMarkup: inlineKeyboard,
                    cancellationToken: cancellationToken);
        }
    }
}
