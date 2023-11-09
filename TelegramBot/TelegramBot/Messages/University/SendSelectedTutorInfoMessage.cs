using Npgsql;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Messages.University
{
    public static partial class Messages
    {
        public static async Task SendSelectedTutorInfoMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string subjId, string categoryId, string tutorId)
        {
            var connString = "Host=localhost;Port=5432;Username=postgres;Password=123;Database=TelegramBot";

            string text;

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand($"SELECT telegram FROM university_teachers WHERE id = {tutorId}", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        reader.Read();

                        text = reader.GetString(0);
                    }
                }
            }

            var chatId = update.CallbackQuery.Message.Chat.Id;
            var messageId = update.CallbackQuery.Message.MessageId;

            InlineKeyboardMarkup inlineKeyboard = new(new[]
                        {
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: $"back_university_tutor&{tutorId}&{categoryId}&{subjId}")
                            }
                        });
            try
            {
                await botClient.EditMessageTextAsync(
                   chatId: chatId,
                   messageId: messageId,
                   text: text,
                   replyMarkup: inlineKeyboard,
                   cancellationToken: cancellationToken);
            }
            catch { }
        }
    }
}
