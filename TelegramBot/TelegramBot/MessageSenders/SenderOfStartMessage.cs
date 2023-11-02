using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.MessageSenders
{
    public static class SenderOfStartMessage
    {
        public static async void SendStartMessage(ITelegramBotClient botClient, Message message)
        {
            var chatId = message.Chat.Id;

            CommandHandlers.UsersState[chatId] = UserStates.MainMenu;

            List<string> buttonLabels = CommandHandlers.StartCommandHandlers.Select(kvp => kvp.Key).ToList();
            List<KeyboardButton[]> keyboardRows = new List<KeyboardButton[]>();

            foreach (string label in buttonLabels)
            {
                if (label != "/start" && label != "Назад" && label != "Вернуться в главное меню")
                {
                    KeyboardButton[] row = new KeyboardButton[] { new KeyboardButton(label) };
                    keyboardRows.Add(row);
                }
            }
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(keyboardRows)
            {
                ResizeKeyboard = true
            };

            var text = "Вас привествует электронный помощник по поиску репетиторов.\n\nДанный бот работает в тестом режиме, если есть какие-то пожелания, то пишите @FindTutor_Support.\n\nДля продолжения нажмите кнопку из меню.";

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: text,
                replyMarkup: replyKeyboardMarkup);
        }
    }
}
