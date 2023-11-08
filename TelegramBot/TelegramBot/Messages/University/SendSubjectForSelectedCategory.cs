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
        public static async Task SendSubjectForSelectedCategoryAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string categoryId)
        {

            var connString = "Host=localhost;Port=5432;Username=postgres;Password=123;Database=TelegramBot";

            Dictionary<int, string> subjects = new Dictionary<int, string>();

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand($"SELECT id, subject FROM university_subjects WHERE categoryid = {categoryId}", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subjects[reader.GetInt32(0)] = (reader.GetString(1));
                        }
                    }
                }
            }

            var chatId = new long();
            var messageId = update.CallbackQuery.Message.MessageId;

            if (update.CallbackQuery != null)
            {
                chatId = update.CallbackQuery.From.Id;
            }
            else
            {
                chatId = update.Message.Chat.Id;
            }

            if (Users.MessageToDelete.ContainsKey(chatId))
            {
                var messageIdsToDelete = Users.MessageToDelete[chatId];
                int tutorsCount = Users.MessageToDelete[chatId];
                for (int i = 0; i <  tutorsCount; i++) 
                {
                    int delmessage = messageId - 1 - i;

                    await botClient.DeleteMessageAsync(chatId, delmessage);
                }
                Users.MessageToDelete[chatId] = 0;
            }


            var inlineKeyBoardButtons = subjects.Select(subject =>
                new[]{ InlineKeyboardButton.WithCallbackData(
                    text: subject.Value,
                    callbackData: $"university_tutors_subj&{subject.Key}&{categoryId}")
                }).ToList();

            inlineKeyBoardButtons.Add(new[]{
                InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "university")
            });

            var inlineKeyboard = new InlineKeyboardMarkup(inlineKeyBoardButtons);

            try
            {
                await botClient.EditMessageTextAsync(
                        chatId: chatId,
                        messageId: messageId,
                        text: "Выберите предмет.",
                        replyMarkup: inlineKeyboard,
                        cancellationToken: cancellationToken);
            }
            catch (Exception ex) { }
        }
    }
}
