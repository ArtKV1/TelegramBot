using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.MessageSenders.School
{
    public static class SenderOfSchoolSubjectsListMessage
    {
        public static async void SendSubjectsListMessage(ITelegramBotClient botClient, Message message)
        {
            var chatId = message.Chat.Id;

            CommandHandlers.UsersState[chatId] = UserStates.FindTutor;

            List<string> buttonLabels = CommandHandlers.SubjectCommandHandlers.Select(kvp => kvp.Key).ToList();
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

            var text = "Выберите предмет из меню по которому хотите найти репетитора.";

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: text,
                replyMarkup: replyKeyboardMarkup);
        }
    }
}
