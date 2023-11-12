using Npgsql;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Messages.School
{
    public static partial class Messages
    {

        public static async Task SendTutorsForSubjectAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string subjId, string categoryId)
        {
            var connString = "Host=localhost;Port=5432;Username=postgres;Password=123;Database=TelegramBot";

            Dictionary<int, List<string>> tutors = new Dictionary<int, List<string>>();

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand($"SELECT id, fullname, description FROM school_teachers WHERE subjectid = {subjId}", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int key = reader.GetInt32(0);
                            string value1 = reader.GetString(1);
                            string value2 = reader.GetString(2);

                            if (!tutors.ContainsKey(key))
                            {
                                tutors[key] = new List<string>();
                            }

                            tutors[key].Add(value1);
                            tutors[key].Add(value2);
                        }
                    }
                }
            }


            var chatId = update.CallbackQuery.Message.Chat.Id;
            var messageId = update.CallbackQuery.Message.MessageId;
            int i = 0;
            foreach (var tutor in tutors)
            {
                i++;
                var tutorId = tutor.Key;
                var textList = tutor.Value;
                var text = $"{textList[0]}\n\n{textList[1]}";

                InlineKeyboardMarkup inlineKeyboardt = new(new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Выбрать", callbackData: $"school_selected_tutor&{tutorId}&{categoryId}&{subjId}"),
                    }
                });

                Message message = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: text,
                    replyMarkup: inlineKeyboardt,
                    cancellationToken: cancellationToken);

                int messageIdToDel = message.MessageId;

                if (!Users.MessageToDelete.ContainsKey(chatId))
                {
                    Users.MessageToDelete[chatId] = new List<int>();
                }

                Users.MessageToDelete[chatId].Add(messageIdToDel);
            }

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: $"school_subj&{categoryId}"),
                }
            });

            await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Нажмите чтобы вернуться.",
                    replyMarkup: inlineKeyboard,
                    cancellationToken: cancellationToken
                    );
            try
            {
                await botClient.DeleteMessageAsync(
                    chatId: chatId,
                    messageId: messageId,
                    cancellationToken: cancellationToken
                    );
            }
            catch
            {

            }
        }
    }
}
