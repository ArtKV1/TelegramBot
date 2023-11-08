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
        public static async Task SendStartMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
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
                    InlineKeyboardButton.WithCallbackData(text: "Найти репетитора", callbackData: "find_tutor"),
                    InlineKeyboardButton.WithCallbackData(text: "Стать репетитором", callbackData: "become_tutor")
                }
            });

            await botClient.EditMessageTextAsync(
                    chatId: chatId,
                    messageId: messageId,
                    text: "Вас привествует электронный помощник по поиску репетиторов.\n\nДанный бот работает в тестом режиме, если есть какие-то пожелания, то пишите @FindTutor_Support.",
                    replyMarkup: inlineKeyboard,
                    cancellationToken: cancellationToken);
        }
    }
}
