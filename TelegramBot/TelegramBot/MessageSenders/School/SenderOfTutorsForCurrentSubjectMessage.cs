using Npgsql;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.MessageSenders.School
{
    public static class SenderOfTutorsForCurrentSubjectMessage
    {
        public static async void SendTutorsForSubjectMessage(ITelegramBotClient botClient, Message message)
        {
            var chatId = message.Chat.Id;
            var subject = message.Text;

            CommandHandlers.UsersState[chatId] = UserStates.SelectedSubject;
            List<string> tutors = GetAllTutorsForOneSubject(subject, chatId);
            if (tutors.Count == 0)
            {
                var text = "По данному предмету пока что нет репетиторов.\n\nЧтобы вернуться нажмите кнопку \"Назад\"";

                ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton("Назад")
                })
                {
                    ResizeKeyboard = true
                };


                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: text,
                    replyMarkup: replyKeyboardMarkup);
            }
            else
            {
                List<int> buttonLabels = CommandHandlers.TutorForCurrentSubjectCommandHandlers[chatId].Select(kvp => kvp.Key).ToList();
                List<KeyboardButton[]> keyboardRows = new List<KeyboardButton[]>();

                foreach (int label in buttonLabels)
                {
                    if (label != int.MaxValue)
                    {
                        KeyboardButton[] row = new KeyboardButton[] { new KeyboardButton(label.ToString()) };
                        keyboardRows.Add(row);
                    }
                }
                KeyboardButton[] back = new KeyboardButton[] { new KeyboardButton("Назад") };
                keyboardRows.Add(back);
                ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(keyboardRows)
                {
                    ResizeKeyboard = true
                };

                foreach (var tutor in tutors)
                {

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: tutor);
                }

                var text = "Выберите репетитора из меню или нажмите на кнопку \"Назад\" для выбора другого предмета.";

                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: text,
                    replyMarkup: replyKeyboardMarkup);
            }
        }

        private static List<string> GetAllTutorsForOneSubject(string subject, long userId)
        {
            var connString = "Host=localhost;Port=5432;Username=postgres;Password=123;Database=TelegramBot";
            var conn = new NpgsqlConnection(connString);
            conn.Open();

            var cmd = new NpgsqlCommand($"SELECT idsubject FROM subjects WHERE subject = '{subject}'", conn);
            var reader = cmd.ExecuteReader();
            reader.Read();
            var subjectId = reader.GetInt32(0);
            reader.Close();
            cmd.Dispose();

            cmd = new NpgsqlCommand($"SELECT secondname, firstname, patronumic, age, description, idtutor FROM Tutors WHERE idcommand = {subjectId} order by idtutor", conn);
            reader = cmd.ExecuteReader();



            List<string> tutors = new List<string>();
            CommandHandlers.TutorForCurrentSubjectCommandHandlers[userId] = new Dictionary<int, Action<ITelegramBotClient, Message>>();
            while (reader.Read())
            {
                string patronymic = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                if (patronymic != string.Empty)
                {
                    tutors.Add($"{reader.GetString(0)} {reader.GetString(1)} {patronymic} - {reader.GetString(3)}\n\n{reader.GetString(4)}\n\nНомер на кнопке: {reader.GetInt32(5)}");
                    CommandHandlers.TutorForCurrentSubjectCommandHandlers[userId][reader.GetInt32(5)] = SenderOfTutorInfoMessage.SendTutorInfoMessage;
                }
                else
                {
                    tutors.Add($"{reader.GetString(0)} {reader.GetString(1)} - {reader.GetString(3)}\n\n{reader.GetString(4)}\n\nНомер на кнопке: {reader.GetInt32(5)}");
                    CommandHandlers.TutorForCurrentSubjectCommandHandlers[userId][reader.GetInt32(5)] = SenderOfTutorInfoMessage.SendTutorInfoMessage;
                }
            }
            CommandHandlers.TutorForCurrentSubjectCommandHandlers[userId][int.MaxValue] = SenderOfSchoolSubjectsListMessage.SendSubjectsListMessage;
            conn.Close();
            return tutors;
        }
    }
}
