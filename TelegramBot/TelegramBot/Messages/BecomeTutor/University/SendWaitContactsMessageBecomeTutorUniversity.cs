using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Messages.BecomeTutor.University
{
    public static partial class Messages
    {
        public static async Task SendWaitContactsMessageBecomeTutorUniversity(ITelegramBotClient botClient, Update? update, Update updateUserMessage, CancellationToken cancellationToken, string description, string fullname, string subjId, string categoryId)
        {
            var chatId = new long();
            var messageId = new int();

            if (update == null)
            {
                chatId = updateUserMessage.CallbackQuery.Message.Chat.Id;
                messageId = updateUserMessage.CallbackQuery.Message.MessageId;

                string text = $"Ваше ФИО: {fullname}\n\nВаше описание:\n{description}\n\nВведите контакты в виде:\ntg: @tg\nWhatsApp: +7xxxxxxxxxx\nvk: https://vk.com/vk\nи так далее.";

                InlineKeyboardMarkup inlineKeyboard = new(new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: $"become_tutor_wait_description_university&{fullname}&{subjId}&{categoryId}")
                    }
                });

                if (!Users.previousMessage.ContainsKey(chatId))
                {
                    Users.previousMessage[chatId] = new Update();
                    Users.previousMessage[chatId].CallbackQuery = new CallbackQuery();
                    Users.previousMessage[chatId].CallbackQuery.Message = new Message();
                    Users.previousMessage[chatId].CallbackQuery.Message.Chat = new Chat();
                }
                Users.previousMessage[chatId].CallbackQuery.Data = $"become_tutor_wait_contacts_university&{description}&{fullname}&{subjId}&{categoryId}";
                Users.previousMessage[chatId].CallbackQuery.Message.Chat.Id = chatId;
                Users.previousMessage[chatId].CallbackQuery.Message.MessageId = messageId;

                await botClient.EditMessageTextAsync(
                    chatId: chatId,
                    messageId: messageId,
                    replyMarkup: inlineKeyboard,
                    text: text);
            }
            else
            {
                chatId = update.CallbackQuery.Message.Chat.Id;
                messageId = update.CallbackQuery.Message.MessageId;

                string text = $"Ваше ФИО: {fullname}\n\nВаше описание:\n{description}\n\nВведите контакты в виде:\ntg: @tg\nWhatsApp: +7xxxxxxxxxx\nvk: https://vk.com/vk\nи так далее.";

                InlineKeyboardMarkup inlineKeyboard = new(new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: $"become_tutor_wait_description_university&{fullname}&{subjId}&{categoryId}")
                    }
                });

                Users.previousMessage[chatId].CallbackQuery.Data = $"become_tutor_wait_contacts_university&{description}&{fullname}&{subjId}&{categoryId}";

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
                    text: text);
            }
        }
    }
}
