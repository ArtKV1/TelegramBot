using Npgsql;
using System.Security.Cryptography.X509Certificates;
using Telegram.Bot;
using Telegram.Bot.Types;
using static System.Net.Mime.MediaTypeNames;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Messages.BecomeTutor
{
    public static partial class Messages
    {
        public static async Task SendPreFinalMessageBecomeTutor(ITelegramBotClient botClient, Update update, Update updateUserMessage, CancellationToken cancellationToken, string contacts, string description, string fullname, string subjId, string categoryId, string educationalInstitution)
        {
            var connString = "Host=localhost;Port=5432;Username=postgres;Password=123;Database=TelegramBot";

            string result = "Ваша заявка:\n\n";

            await using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                if (educationalInstitution == "school")
                {

                    result += "Учебное заведение: Школа\n";

                    await using (var cmd = new NpgsqlCommand($"SELECT category FROM categories WHERE id = {categoryId}", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result += $"Категория: {reader.GetString(0)}\n";
                            }
                        }
                    }
                    await using (var cmd = new NpgsqlCommand($"SELECT subject FROM school_subjects WHERE id = {subjId}", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result += $"Предмет: {reader.GetString(0)}\n";
                            }
                        }
                    }
                }
                else if (educationalInstitution == "university")
                {
                    result += "Учебное заведение: Университет\n";

                    await using (var cmd = new NpgsqlCommand($"SELECT category FROM categories WHERE id = {categoryId}", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result += $"Категория: {reader.GetString(0)}\n";
                            }
                        }
                    }
                    await using (var cmd = new NpgsqlCommand($"SELECT subject FROM university_subjects WHERE id = {subjId}", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result += $"Предмет: {reader.GetString(0)}\n";
                            }
                        }
                    }
                }
            }

            result += $"Ваше ФИО: {fullname}\nВаше описание:\n{description}\n\nВаши контакты:\n{contacts}";

            var chatId = update.CallbackQuery.Message.Chat.Id;
            var messageId = update.CallbackQuery.Message.MessageId;

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Отправить", callbackData: $"send_message_to_admin&{educationalInstitution}&{contacts}&{description}&{fullname}&{subjId}&{categoryId}")
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: $"become_tutor_wait_contacts_school&{description}&{fullname}&{subjId}&{categoryId}")
                }
            });

            Users.previousMessage[chatId].CallbackQuery.Data = $"become_tutor_wait_prefinal&{contacts}&{description}&{fullname}&{subjId}&{categoryId}";

            try
            {
                await botClient.DeleteMessageAsync(
                    chatId: chatId,
                    messageId: updateUserMessage.Message.MessageId);
            }
            catch { }

            await botClient.EditMessageTextAsync(
                chatId: chatId,
                messageId: messageId,
                replyMarkup: inlineKeyboard,
                text: result);
        }
    }
}
