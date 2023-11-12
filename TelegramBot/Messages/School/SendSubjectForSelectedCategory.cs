using Npgsql;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Messages.School
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

                using (var cmd = new NpgsqlCommand($"SELECT id, subject FROM school_subjects WHERE categoryid = {categoryId}", conn))
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

            var chatId = update.CallbackQuery.Message.Chat.Id;
            var messageId = update.CallbackQuery.Message.MessageId;

            Users.LastMessage[chatId] = messageId;

            if (Users.MessageToDelete.ContainsKey(chatId))
            {
                foreach (var deleteMessageId in Users.MessageToDelete[chatId]) 
                {
                    try
                    {
                        await botClient.DeleteMessageAsync(chatId, deleteMessageId);
                    }
                    catch(Exception ex) 
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
                Users.MessageToDelete.Remove(chatId);
            }


            var inlineKeyBoardButtons = subjects.Select(subject =>
                new[]{ InlineKeyboardButton.WithCallbackData(
                    text: subject.Value,
                    callbackData: $"school_tutors_subj&{subject.Key}&{categoryId}")
                }).ToList();

            inlineKeyBoardButtons.Add(new[]{
                InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "school")
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
