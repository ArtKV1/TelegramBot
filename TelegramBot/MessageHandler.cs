using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

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

                if(!Users.UsersState.ContainsKey(chatId))
                {
                    Users.UsersState[chatId] = UserStates.Other;
                }

                Console.WriteLine($"{chatId}\t|\t{callbackData}\t|\t{update.CallbackQuery.Message.MessageId}");
                if (callbackData == "main_menu")
                {
                    await Messages.Messages.SendStartMessageAsync(botClient, update, cancellationToken);
                }
                else if (callbackData == "find_tutor")
                {
                    await Messages.Messages.SendFindTutorMessageAsync(botClient, update, cancellationToken);
                }
                else if (callbackData == "school")
                {
                    await Messages.School.Messages.SendCategoriesMessageAsync(botClient, update, cancellationToken);
                }
                else if (callbackData.Contains("school_subj"))
                {
                    var categoryId = callbackData.Split('&')[1];

                    await Messages.School.Messages.SendSubjectForSelectedCategoryAsync(botClient, update, cancellationToken, categoryId);
                }
                else if (callbackData.Contains("school_tutors_subj"))
                {
                    var subjId = callbackData.Split('&')[1];
                    var categoryId = callbackData.Split('&')[2];
                    await Messages.School.Messages.SendTutorsForSubjectAsync(botClient, update, cancellationToken, subjId, categoryId);
                }
                else if (callbackData.Contains("school_selected_tutor"))
                {
                    var tutorId = callbackData.Split('&')[1];
                    var categoryId = callbackData.Split('&')[2];
                    var subjId = callbackData.Split('&')[3];

                    await Messages.School.Messages.SendSelectedTutorInfoMessageAsync(botClient, update, cancellationToken, subjId, categoryId, tutorId);
                }
                else if (callbackData.Contains("back_school_tutor"))
                {
                    var tutorId = callbackData.Split('&')[1];
                    var categoryId = callbackData.Split('&')[2];
                    var subjId = callbackData.Split('&')[3];

                    await Messages.School.Messages.SendTutorForSubjectAsync(botClient, update, cancellationToken, categoryId, subjId, tutorId);
                }
                else if (callbackData == "university")
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
                else if (callbackData.Contains("back_university_tutor"))
                {
                    var tutorId = callbackData.Split('&')[1];
                    var categoryId = callbackData.Split('&')[2];
                    var subjId = callbackData.Split('&')[3];

                    await Messages.University.Messages.SendTutorForSubjectAsync(botClient, update, cancellationToken, categoryId, subjId, tutorId);
                }
                else if (callbackData == "become_tutor")
                {
                    await Messages.Messages.SendBecomeTutorMessageAsync(botClient, update, cancellationToken);
                }
                else if (callbackData == "send_adm")
                {
                    await Messages.BecomeTutor.Messages.SendMessageToAdminAsync(botClient, update, cancellationToken);
                }
                else if (callbackData.Contains("admin"))
                {
                    var status = callbackData.Split("&")[1];
                    var userId = callbackData.Split("&")[2];

                    await Messages.BecomeTutor.Messages.SendApproveOrRejectMessage(botClient, update, cancellationToken, status, userId);
                }
            }
            else
            {
                if (update.Message != null)
                {
                    Console.WriteLine($"{update.Message.Chat.Id}\t|\t{update.Message.Text}");

                    var chatId = update.Message.Chat.Id;
                    var messageId = update.Message.MessageId;

                    if (!Users.UsersState.ContainsKey(chatId))
                    {
                        Users.UsersState[chatId] = UserStates.Other;
                    }
                    if (Users.UsersState[chatId] == UserStates.Other) 
                    {
                        InlineKeyboardMarkup inlineKeyboard = new(new[]
                        {
                            new []
                            {
                                 InlineKeyboardButton.WithCallbackData(text: "Найти репетитора", callbackData: "find_tutor"),
                                 InlineKeyboardButton.WithCallbackData(text: "Стать репетитором", callbackData: "become_tutor")
                            }
                        });

                        await botClient.DeleteMessageAsync(
                                chatId: chatId,
                                messageId: messageId);

                        await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: "Вас привествует электронный помощник по поиску репетиторов.\n\nЕсли здесь нет вашего предмета, то напишите сюда: @FindTutor_Support.",
                                replyMarkup: inlineKeyboard,
                                cancellationToken: cancellationToken);
                    }
                    else
                    {
                        if (update.Message.ReplyToMessage != null)
                        {
                            await Messages.BecomeTutor.Messages.SendConfirmationMessageAsync(botClient, update, cancellationToken);
                        }
                        else
                        {

                        }
                    }
                }
            }
        }
    }
}
