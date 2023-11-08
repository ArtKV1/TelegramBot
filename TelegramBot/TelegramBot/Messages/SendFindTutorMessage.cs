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
        public static async Task SendFindTutorMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
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

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Школа", callbackData: "school"),
                    InlineKeyboardButton.WithCallbackData(text: "Университет", callbackData: "university"),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "main_menu")
                }

            });

            await botClient.EditMessageTextAsync(
                    chatId: chatId,
                    messageId: messageId,
                    text: "Выберите учебное заведение.",
                    replyMarkup: inlineKeyboard,
                    cancellationToken: cancellationToken);
        }
    }
}
