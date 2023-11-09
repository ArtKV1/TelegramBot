using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Messages
{
    public static partial class Messages
    {
        public static async Task SendMessageToAdmin(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string educationalInstitution, string contacts, string description, string fullname, string subjId, string categoryId)
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;
            var messageId = update.CallbackQuery.Message.MessageId;

            var text = "Ваша заявка отправлена.\nВремя проверки до 24 часов. Если заявка будет отклонена или одобрена, то вам придёт сообщение.";

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: $"main_menu"),
                }
            });

            Users.previousMessage[chatId] = new Update();

            await botClient.EditMessageTextAsync(
                chatId: chatId,
                messageId: messageId,
                text: text,
                replyMarkup: inlineKeyboard);

            var connString = "Host=localhost;Port=5432;Username=postgres;Password=123;Database=TelegramBot";

            var result = $"Одоброение заявки.\n\n";

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

            result += $"ФИО: {fullname}\nОписание:\n{description}\n\nКонтакты:\n{contacts}";

            InlineKeyboardMarkup inlineKeyboardtoAdmin = new(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Одобрить", callbackData: $"approve_or_reject&1&{chatId}&{educationalInstitution}&{contacts}&{description}&{fullname}&{subjId}&{categoryId}"),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Отклонить", callbackData: $"approve_or_reject&0&{chatId}"),
                }
            });

            await botClient.SendTextMessageAsync(
            chatId: 6522857795,
            text: result,
            replyMarkup: inlineKeyboardtoAdmin);
        }
    }
}
