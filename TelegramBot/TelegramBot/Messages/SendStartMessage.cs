using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Messages
{
    public static partial class Messages
    {
        public static async Task SendStartMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;
            var messageId = update.CallbackQuery.Message.MessageId;

            Users.UsersState[chatId] = UserStates.Other;

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Найти репетитора", callbackData: "find_tutor"),
                    InlineKeyboardButton.WithCallbackData(text: "Стать репетитором", callbackData: "become_tutor")
                }
            });

            await botClient.EditMessageTextAsync(
                    chatId: chatId,
                    messageId: messageId,
                    text: "Вас привествует электронный помощник по поиску репетиторов.\n\nЕсли здесь нет вашего предмета, то напишите сюда: @FindTutor_Support.",
                    replyMarkup: inlineKeyboard,
                    cancellationToken: cancellationToken);
        }
    }
}
