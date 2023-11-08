using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            var chatId = update.CallbackQuery.From.Id;
            var messageId = update.CallbackQuery.Message.MessageId;
            if (Users.MessageToDelete.ContainsKey(chatId))
            {
                var messageIdsToDelete = Users.MessageToDelete[chatId];
                int tutorsCount = Users.MessageToDelete[chatId] + 1;
                for (int i = 0; i < tutorsCount; i++)
                {
                    int delMessageNext = messageId + 1 + i;

                    try
                    {
                        await botClient.DeleteMessageAsync(chatId, delMessageNext);
                    }
                    catch
                    {
                        break;
                    }
                }
                for (int i = 0; i < tutorsCount; i++)
                {
                    int delMessagePrev = messageId - 1 - i;

                    try
                    {
                        await botClient.DeleteMessageAsync(chatId, delMessagePrev);
                    }
                    catch
                    {
                        break;
                    }
                }
                Users.MessageToDelete[chatId] = 0;
            }
            InlineKeyboardMarkup inlineKeyboard = new(new[]
                        {
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: $"university_tutors_subj&{subjId}&{categoryId}")
                            }
                        });

            await botClient.EditMessageTextAsync(
               chatId: chatId,
               messageId: messageId,
               text: text,
               replyMarkup: inlineKeyboard,
               cancellationToken: cancellationToken);
        }
    }
}
