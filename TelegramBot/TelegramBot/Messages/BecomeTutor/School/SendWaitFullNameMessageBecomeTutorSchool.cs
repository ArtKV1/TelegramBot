using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Messages.BecomeTutor.School
{
    public static partial class Messages
    {
        public static async Task SendWaitFullNameMessageBecomeTutorSchool(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string subjId, string categoryId)
        {
            var messageId = update.CallbackQuery.Message.MessageId;
            var chatId = update.CallbackQuery.Message.Chat.Id;

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: $"become_tutor_sch_subj&{categoryId}"),
                }
            });

            await botClient.EditMessageTextAsync(
                chatId: chatId,
                messageId: messageId,
                text: "Введите ФИО.",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);

            var callbackData = $"become_tutor_sch_subj&{categoryId}";

            if (!Users.previousMessage.ContainsKey(chatId)) 
            {
                Users.previousMessage[chatId] = new Update();
            }

            Users.previousMessage[chatId] = update;
        }
    }
}
