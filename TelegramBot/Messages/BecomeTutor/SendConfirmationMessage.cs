using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Messages.BecomeTutor
{
    public static partial class Messages
    {
        public static async Task SendConfirmationMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var chatId = update.Message.Chat.Id;
            var messageId = update.Message.MessageId;
            var messageBotId = update.Message.ReplyToMessage.MessageId;

            try
            {
                await botClient.DeleteMessageAsync(
                    chatId: chatId,
                    messageId: messageBotId);
            } catch { }

            try
            {
                await botClient.DeleteMessageAsync(
                    chatId: chatId,
                    messageId: messageId);
            } catch { }

            var text = $"Проверьте все данные перед отправкой.\n\n{update.Message.Text}";

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Отправить", callbackData: "send_adm"),
                        InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "become_tutor")
                    }
            });

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: text,
                replyMarkup: inlineKeyboard);
        }
    }
}
