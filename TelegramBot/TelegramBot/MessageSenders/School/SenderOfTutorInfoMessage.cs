using Npgsql;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.MessageSenders.School
{
    public static class SenderOfTutorInfoMessage
    {
        public static async void SendTutorInfoMessage(ITelegramBotClient botClient, Message message)
        {
            var chatId = message.Chat.Id;

            CommandHandlers.UsersState[chatId] = UserStates.SelectedTutor;
            var text = GetTutorInfo(message.Text);

            List<string> buttonLabels = CommandHandlers.SelectedTutorCommandHandlers.Select(kvp => kvp.Key).ToList();
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

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: text,
                replyMarkup: replyKeyboardMarkup);
        }

        private static string GetTutorInfo(string tutor)
        {
            List<string> list = tutor.Split(" ").ToList();

            string result;

            var connString = "Host=localhost;Port=5432;Username=postgres;Password=123;Database=TelegramBot";
            var conn = new NpgsqlConnection(connString);
            conn.Open();

            if (list.Count == 2)
            {
                var cmd = new NpgsqlCommand($"SELECT phonenumber, vk, telegram, discord, skype FROM tutors WHERE secondname = '{list[0]}' and firstname = '{list[1]}'", conn);
                var reader = cmd.ExecuteReader();
                reader.Read();
                var phonenumber = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                var vk = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                var telegram = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                var discord = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                var skype = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                result = $"Номер телефона: {phonenumber}\nСтраница в VK: {vk}\nTelegram: {telegram}\nDiscord: {discord}\nSkype: {skype}";
                reader.Close();
                cmd.Dispose();
            }
            else
            {
                var cmd = new NpgsqlCommand($"SELECT phonenumber, vk, telegram, discord, skype FROM tutors WHERE idtutor = {list[0]}", conn);
                var reader = cmd.ExecuteReader();
                reader.Read();
                var phonenumber = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                var vk = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                var telegram = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                var discord = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                var skype = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                result = $"Номер телефона: {phonenumber}\nСтраница в VK: {vk}\nTelegram: {telegram}\nDiscord: {discord}\nSkype: {skype}";
                reader.Close();
                cmd.Dispose();
            }
            return result;
        }
    }
}
