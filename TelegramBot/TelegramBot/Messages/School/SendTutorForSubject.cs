using Npgsql;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Messages.School
{
    public static partial class Messages
    {
        public static async Task SendTutorForSubjectAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string categoryId, string subjId, string tutorId)
        {

            var connString = "Host=localhost;Port=5432;Username=postgres;Password=123;Database=TelegramBot";

            List<string> tutor = new List<string>();

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand($"SELECT id, fullname, description FROM school_teachers WHERE id = {tutorId}", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        int key = reader.GetInt32(0);
                        string value1 = reader.GetString(1);
                        string value2 = reader.GetString(2);

                        tutor.Add(key.ToString());
                        tutor.Add(value1);
                        tutor.Add(value2);
                    }
                }
            }

            var chatId = update.CallbackQuery.Message.Chat.Id;
            var messageId = update.CallbackQuery.Message.MessageId;

            var text = $"{tutor[1]}\n\n{tutor[2]}";

            InlineKeyboardMarkup inlineKeyboardt = new(new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Выбрать", callbackData: $"school_selected_tutor&{tutorId}&{categoryId}&{subjId}"),
                    }
                });

            await botClient.EditMessageTextAsync(
                    chatId: chatId,
                    messageId: messageId,
                    text: text,
                    replyMarkup: inlineKeyboardt,
                    cancellationToken: cancellationToken);
        }
    }
}
