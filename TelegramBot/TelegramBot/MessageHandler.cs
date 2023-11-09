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
                    await Messages.BecomeTutor.Messages.SendFirstMessageBecomeTutor(botClient, update, cancellationToken);
                }
                else if (callbackData == "become_tutor_school")
                {
                    await Messages.BecomeTutor.School.Messages.SendSecondMessageBecomeTutorSchool(botClient, update, cancellationToken);
                }
                else if (callbackData == "become_tutor_university")
                {
                    await Messages.BecomeTutor.University.Messages.SendSecondMessageBecomeTutorUniversity(botClient, update, cancellationToken);
                }
                else if (callbackData.Contains("become_tutor_sch_subj"))
                {
                    var categoryId = callbackData.Split('&')[1];
                    await Messages.BecomeTutor.School.Messages.SendSubjectsForSelectedCategoryBecomeTutorSchool(botClient, update, cancellationToken, categoryId);
                }
                else if (callbackData.Contains("become_tutor_wait_fio_school"))
                {
                    var subjId = callbackData.Split('&')[1];
                    var categoryId = callbackData.Split('&')[2];
                    await Messages.BecomeTutor.School.Messages.SendWaitFullNameMessageBecomeTutorSchool(botClient, update, cancellationToken, subjId, categoryId);
                }
                else if (callbackData.Contains("become_tutor_wait_description_school"))
                {
                    var fullname = callbackData.Split("&")[1];
                    var subjId = callbackData.Split('&')[2];
                    var categoryId = callbackData.Split('&')[3];
                    Update? upd = null;

                    await Messages.BecomeTutor.School.Messages.SendWaitDescriptionMessageBecomeTutorSchool(botClient, upd, update, cancellationToken, fullname, subjId, categoryId);
                }
                else if (callbackData.Contains("become_tutor_wait_contacts_school"))
                {
                    var description = callbackData.Split("&")[1];
                    var fullname = callbackData.Split("&")[2];
                    var subjId = callbackData.Split('&')[3];
                    var categoryId = callbackData.Split('&')[4];
                    Update? upd = null;

                    await Messages.BecomeTutor.School.Messages.SendWaitContactsMessageBecomeTutorSchool(botClient, upd, update, cancellationToken, description, fullname, subjId, categoryId);
                }
                else if (callbackData.Contains("become_tutor_uni_subj"))
                {
                    var categoryId = callbackData.Split('&')[1];
                    await Messages.BecomeTutor.University.Messages.SendSubjectsForSelectedCategoryBecomeTutorUniversity(botClient, update, cancellationToken, categoryId);
                }
                else if (callbackData.Contains("become_tutor_wait_fio_university"))
                {
                    var subjId = callbackData.Split('&')[1];
                    var categoryId = callbackData.Split('&')[2];
                    await Messages.BecomeTutor.University.Messages.SendWaitFullNameMessageBecomeTutorUniversity(botClient, update, cancellationToken, subjId, categoryId);
                }
                else if (callbackData.Contains("become_tutor_wait_description_university"))
                {
                    var fullname = callbackData.Split("&")[1];
                    var subjId = callbackData.Split('&')[2];
                    var categoryId = callbackData.Split('&')[3];
                    Update? upd = null;

                    await Messages.BecomeTutor.University.Messages.SendWaitDescriptionMessageBecomeTutorUniversity(botClient, upd, update, cancellationToken, fullname, subjId, categoryId);
                }
                else if (callbackData.Contains("become_tutor_wait_contacts_university"))
                {
                    var description = callbackData.Split("&")[1];
                    var fullname = callbackData.Split("&")[2];
                    var subjId = callbackData.Split('&')[3];
                    var categoryId = callbackData.Split('&')[4];
                    Update? upd = null;

                    await Messages.BecomeTutor.University.Messages.SendWaitContactsMessageBecomeTutorUniversity(botClient, upd, update, cancellationToken, description, fullname, subjId, categoryId);
                }
                else if (callbackData.Contains("send_message_to_admin"))
                {
                    var educationalInstitution = callbackData.Split("&")[1];
                    var contacts = callbackData.Split("&")[2];
                    var description = callbackData.Split("&")[3];
                    var fullname = callbackData.Split("&")[4];
                    var subjId = callbackData.Split("&")[5];
                    var categoryId = callbackData.Split("&")[6];

                    await Messages.Messages.SendMessageToAdmin(botClient, update, cancellationToken, educationalInstitution, contacts, description, fullname, subjId, categoryId);
                }
                else if (callbackData.Contains("approve_or_reject"))
                {
                    var status = callbackData.Split("&")[1];
                    var userId = callbackData.Split("&")[2];
                    var educationalInstitution = callbackData.Split("&").ElementAtOrDefault(3);
                    var contacts = callbackData.Split("&").ElementAtOrDefault(4);
                    var description = callbackData.Split("&").ElementAtOrDefault(5);
                    var fullname = callbackData.Split("&").ElementAtOrDefault(6);
                    var subjId = callbackData.Split("&").ElementAtOrDefault(7);

                    await Messages.Messages.SendApproveOrRejectMessage(botClient, update, cancellationToken, status, userId, educationalInstitution, contacts, description, fullname, subjId);
                }
            }
            else
            {
                var chatId = update.Message.Chat.Id;
                if (update.Message != null)
                {
                    if (Users.previousMessage.ContainsKey(chatId))
                    {
                        var upd = Users.previousMessage[chatId];

                        if (upd.CallbackQuery != null)
                        {
                            if (upd.CallbackQuery.Data.Contains("become_tutor_wait_fio_school"))
                            {
                                var fullname = update.Message.Text;
                                var subjId = upd.CallbackQuery.Data.Split('&')[1];
                                var categoryId = upd.CallbackQuery.Data.Split('&')[2];

                                await Messages.BecomeTutor.School.Messages.SendWaitDescriptionMessageBecomeTutorSchool(botClient, upd, update, cancellationToken, fullname, subjId, categoryId);
                            }
                            else if (upd.CallbackQuery.Data.Contains("become_tutor_wait_description_school"))
                            {
                                var description = update.Message.Text;
                                var fullname = upd.CallbackQuery.Data.Split('&')[1];
                                var subjId = upd.CallbackQuery.Data.Split('&')[2];
                                var categoryId = upd.CallbackQuery.Data.Split('&')[3];

                                await Messages.BecomeTutor.School.Messages.SendWaitContactsMessageBecomeTutorSchool(botClient, upd, update, cancellationToken, description, fullname, subjId, categoryId);
                            }
                            else if (upd.CallbackQuery.Data.Contains("become_tutor_wait_contacts_school"))
                            {
                                var contacts = update.Message.Text;
                                var description = upd.CallbackQuery.Data.Split('&')[1];
                                var fullname = upd.CallbackQuery.Data.Split('&')[2];
                                var subjId = upd.CallbackQuery.Data.Split('&')[3];
                                var categoryId = upd.CallbackQuery.Data.Split('&')[4];
                                var school = "school";

                                await Messages.BecomeTutor.Messages.SendPreFinalMessageBecomeTutor(botClient, upd, update, cancellationToken, contacts, description, fullname, subjId, categoryId, school);
                            }
                            else if (upd.CallbackQuery.Data.Contains("become_tutor_wait_fio_university"))
                            {
                                var fullname = update.Message.Text;
                                var subjId = upd.CallbackQuery.Data.Split('&')[1];
                                var categoryId = upd.CallbackQuery.Data.Split('&')[2];

                                await Messages.BecomeTutor.University.Messages.SendWaitDescriptionMessageBecomeTutorUniversity(botClient, upd, update, cancellationToken, fullname, subjId, categoryId);
                            }
                            else if (upd.CallbackQuery.Data.Contains("become_tutor_wait_description_university"))
                            {
                                var description = update.Message.Text;
                                var fullname = upd.CallbackQuery.Data.Split('&')[1];
                                var subjId = upd.CallbackQuery.Data.Split('&')[2];
                                var categoryId = upd.CallbackQuery.Data.Split('&')[3];

                                await Messages.BecomeTutor.University.Messages.SendWaitContactsMessageBecomeTutorUniversity(botClient, upd, update, cancellationToken, description, fullname, subjId, categoryId);
                            }
                            else if (upd.CallbackQuery.Data.Contains("become_tutor_wait_contacts_university"))
                            {
                                var contacts = update.Message.Text;
                                var description = upd.CallbackQuery.Data.Split('&')[1];
                                var fullname = upd.CallbackQuery.Data.Split('&')[2];
                                var subjId = upd.CallbackQuery.Data.Split('&')[3];
                                var categoryId = upd.CallbackQuery.Data.Split('&')[4];
                                var school = "university";

                                await Messages.BecomeTutor.Messages.SendPreFinalMessageBecomeTutor(botClient, upd, update, cancellationToken, contacts, description, fullname, subjId, categoryId, school);
                            }
                            else
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

                                await botClient.SendTextMessageAsync(
                                        chatId: chatId,
                                        text: "Вас привествует электронный помощник по поиску репетиторов.\n\nДанный бот работает в тестом режиме, если есть какие-то пожелания, то пишите @FindTutor_Support.",
                                        replyMarkup: inlineKeyboard,
                                        cancellationToken: cancellationToken);
                            }
                        }
                        else
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

                            await botClient.SendTextMessageAsync(
                                    chatId: chatId,
                                    text: "Вас привествует электронный помощник по поиску репетиторов.\n\nДанный бот работает в тестом режиме, если есть какие-то пожелания, то пишите @FindTutor_Support.",
                                    replyMarkup: inlineKeyboard,
                                    cancellationToken: cancellationToken);
                        }
                    }
                    else
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

                        await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: "Вас привествует электронный помощник по поиску репетиторов.\n\nДанный бот работает в тестом режиме, если есть какие-то пожелания, то пишите @FindTutor_Support.",
                                replyMarkup: inlineKeyboard,
                                cancellationToken: cancellationToken);
                    }
                }
            }
        }
    }
}
