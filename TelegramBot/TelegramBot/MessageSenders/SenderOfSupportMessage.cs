using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.MessageSenders
{
    public static class SenderOfSupportMessage
    {
        public static async void SendSupportMessage(ITelegramBotClient botClient, Message message)
        {
            var chatId = message.Chat.Id;
            CommandHandlers.UsersState[chatId] = UserStates.Support;

            List<string> buttonLabels = CommandHandlers.SupportCommandHandlers.Select(kvp => kvp.Key).ToList();
            List<KeyboardButton[]> keyboardRows = new List<KeyboardButton[]>();

            foreach (string label in buttonLabels)
            {
                KeyboardButton[] row = new KeyboardButton[] { new KeyboardButton(label) };
                keyboardRows.Add(row);
            }
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(keyboardRows)
            {
                ResizeKeyboard = true
            };

            var text = "По всем вопросам обращайтесь к @FindTutor_Support";

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: text,
                replyMarkup: replyKeyboardMarkup);
        }
    }
}
