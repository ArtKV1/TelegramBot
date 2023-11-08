using NickBuhro.Translit;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Messages.School
{
    public static partial class Messages
    {
        public static async Task SendCategoriesMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var connString = "Host=localhost;Port=5432;Username=postgres;Password=123;Database=TelegramBot";

            Dictionary<int, string> categories = new Dictionary<int, string>();

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand("SELECT id, category FROM categories", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories[reader.GetInt32(0)] = (reader.GetString(1));
                        }
                    }
                }
            }



            var chatId = update.CallbackQuery.Message.Chat.Id;
            var messageId = update.CallbackQuery.Message.MessageId;

            if (update.CallbackQuery != null)
            {
                chatId = update.CallbackQuery.From.Id;
            }
            else
            {
                chatId = update.Message.Chat.Id;
            }

            var inlineKeyBoardButtons = categories.Select(category =>
                new[]{ InlineKeyboardButton.WithCallbackData(
                    text: category.Value,
                    callbackData: $"school_subj&{category.Key}")
                }).ToList();

            inlineKeyBoardButtons.Add(new[]{ 
                InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "find_tutor") 
            });

            var inlineKeyboard = new InlineKeyboardMarkup(inlineKeyBoardButtons);

            await botClient.EditMessageTextAsync(
                    chatId: chatId,
                    messageId: messageId,
                    text: "Выберите категорию предметов.",
                    replyMarkup: inlineKeyboard,
                    cancellationToken: cancellationToken);
        }
    }
}
