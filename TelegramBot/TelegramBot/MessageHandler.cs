using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Xsl;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;

namespace TelegramBot
{
    public class MessageHandler
    {
        public static async Task SendMessageHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.CallbackQuery != null)
            {
                var callbackData = update.CallbackQuery.Data;
                var chatId = update.CallbackQuery.From.Id;
                var messageId = update.CallbackQuery.Message.MessageId;

                Console.WriteLine($"{chatId}\t|\t{callbackData}");
                if (callbackData== "main_menu")
                {
                    await Messages.Messages.SendStartMessageAsync(botClient, update, cancellationToken);
                }
                else if (callbackData == "find_tutor")
                {
                    await Messages.Messages.SendFindTutorMessageAsync(botClient, update, cancellationToken);
                }
                else if (callbackData == "school")
                {
                    await Messages.University.Messages.SendCategoriesMessageAsync(botClient, update, cancellationToken);
                }
                else if(callbackData == "university")
                {
                    await Messages.University.Messages.SendCategoriesMessageAsync(botClient, update, cancellationToken);
                }
                else if (callbackData.Contains("university_subj"))
                {
                    var categoryId = callbackData.Split('&')[1];

                    await Messages.University.Messages.SendSubjectForSelectedCategoryAsync(botClient, update, cancellationToken, categoryId);
                }
                else if (callbackData.Contains("university_tutors_subj"))
                {
                    var subjId = callbackData.Split('&')[1];
                    var categoryId = callbackData.Split('&')[2];
                    await Messages.University.Messages.SendTutorsForSubjectAsync(botClient, update, cancellationToken, subjId, categoryId);
                }
                else if (callbackData.Contains("university_selected_tutor"))
                {
                    var tutorId = callbackData.Split('&')[1];
                    var categoryId = callbackData.Split('&')[2];
                    var subjId = callbackData.Split('&')[3];

                    await Messages.University.Messages.SendSelectedTutorInfoMessageAsync(botClient, update, cancellationToken, subjId, categoryId, tutorId);
                }
            }
            else
            {
                if (update.Message != null)
                {
                    Console.WriteLine($"{update.Message.Chat.Id}\t|\t{update.Message.Text}");

                    InlineKeyboardMarkup inlineKeyboard = new(new[]
                    {
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData(text: "Найти репетитора", callbackData: "find_tutor"),
                            InlineKeyboardButton.WithCallbackData(text: "Стать репетитором", callbackData: "become_tutor")
                        }
                    });

                    var userId = update.Message.Chat.Id;

                    await botClient.SendTextMessageAsync(
                            chatId: userId,
                            text: "Вас привествует электронный помощник по поиску репетиторов.\n\nДанный бот работает в тестом режиме, если есть какие-то пожелания, то пишите @FindTutor_Support.",
                            replyMarkup: inlineKeyboard,
                            cancellationToken: cancellationToken);
                }
            }
        }
    }
}
