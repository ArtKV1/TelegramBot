using Npgsql;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.MessageSenders.School;

namespace TelegramBot
{
    public static class MessageHandler
    {
        public static async void BotInWork(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            if (message != null)
            {
                Console.WriteLine($"{message.Chat.Id}\t|\t{message.Text}");
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "В данный момент бот не работает, попробуйте заглянуть позже.",
                    replyMarkup: new ReplyKeyboardRemove());
            }
        }

        private static void FillingCommandHandlers()
        {
            CommandHandlers.AllCommandHandlers = new Dictionary<string, string>();
            CommandHandlers.SubjectCommandHandlers = new Dictionary<string, Action<ITelegramBotClient, Message>>();
            CommandHandlers.SupportCommandHandlers = new Dictionary<string, Action<ITelegramBotClient, Message>>();
            CommandHandlers.SelectedTutorCommandHandlers = new Dictionary<string, Action<ITelegramBotClient, Message>>();
            CommandHandlers.DifficultySelectionCommandHandlers = new Dictionary<string, Action<ITelegramBotClient, Message>>();

            CommandHandlers.AllCommandHandlers["/start"] = "/start";
            CommandHandlers.AllCommandHandlers["Вернуться в главное меню"] = "Вернуться в главное меню";
            CommandHandlers.AllCommandHandlers["Назад"] = "Назад";
            CommandHandlers.AllCommandHandlers["Найти репетитора"] = "Найти репетитора";
            CommandHandlers.AllCommandHandlers["Стать репетитором"] = "Стать репетитором";
            CommandHandlers.AllCommandHandlers["Обратная связь"] = "Обратная связь";

            CommandHandlers.DifficultySelectionCommandHandlers["Школьные предметы"] = SenderOfSchoolSubjectsListMessage.SendSubjectsListMessage;
            /*CommandHandlers.DifficultySelectionCommandHandlers["Предметы ВУЗов"] = SenderOfUniversitySubjectsListMessage.SendSubjectsListMessage;*/

            var connString = "Host=localhost;Port=5432;Username=postgres;Password=123;Database=TelegramBot";
            var conn = new NpgsqlConnection(connString);
            conn.Open();

            var cmd = new NpgsqlCommand("SELECT idtutor FROM tutors", conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var command = reader.GetInt32(0);
                CommandHandlers.AllCommandHandlers[command.ToString()] = command.ToString();
            }

            reader.Close();
            cmd.Dispose();

            cmd = new NpgsqlCommand("SELECT subject FROM subjects", conn);
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var command = reader.GetString(0);
                CommandHandlers.AllCommandHandlers[command] = command;
                CommandHandlers.SubjectCommandHandlers[command] = SenderOfTutorsForCurrentSubjectMessage.SendTutorsForSubjectMessage;
            }
            CommandHandlers.AllCommandHandlers["Здесь нет моего предмета"] = "Здесь нет моего предмета";
            CommandHandlers.SubjectCommandHandlers["Здесь нет моего предмета"] = MessageSenders.SenderOfSupportMessage.SendSupportMessage;
            CommandHandlers.SubjectCommandHandlers["Назад"] = MessageSenders.SenderOfStartMessage.SendStartMessage;

            reader.Close();
            cmd.Dispose();

            CommandHandlers.StartCommandHandlers = new Dictionary<string, Action<ITelegramBotClient, Message>>();

            CommandHandlers.StartCommandHandlers["/start"] = MessageSenders.SenderOfStartMessage.SendStartMessage;
            CommandHandlers.StartCommandHandlers["Назад"] = MessageSenders.SenderOfStartMessage.SendStartMessage;
            CommandHandlers.StartCommandHandlers["Вернуться в главное меню"] = MessageSenders.SenderOfStartMessage.SendStartMessage;
            CommandHandlers.StartCommandHandlers["Найти репетитора"] = SenderOfSchoolSubjectsListMessage.SendSubjectsListMessage;
            CommandHandlers.StartCommandHandlers["Стать репетитором"] = MessageSenders.SenderOfSupportMessage.SendSupportMessage;

            CommandHandlers.SupportCommandHandlers["Назад"] = MessageSenders.SenderOfStartMessage.SendStartMessage;

            CommandHandlers.SelectedTutorCommandHandlers["Назад"] = SenderOfSchoolSubjectsListMessage.SendSubjectsListMessage;
        }

        private static bool IsCommandIsAllowed(long userId, string command)
        {
            UserStates currentState;
            if (CommandHandlers.UsersState.TryGetValue(userId, out currentState))
            {
                if (currentState == UserStates.MainMenu)
                {
                    return CommandHandlers.AllCommandHandlers.ContainsKey(command) && CommandHandlers.StartCommandHandlers.ContainsKey(command);
                }
                else if (currentState == UserStates.FindTutor)
                {
                    return CommandHandlers.AllCommandHandlers.ContainsKey(command) && CommandHandlers.SubjectCommandHandlers.ContainsKey(command);
                }
                else if (currentState == UserStates.Support)
                {
                    return CommandHandlers.AllCommandHandlers.ContainsKey(command) && command == "Назад";
                }
                else if (currentState == UserStates.SelectedSubject)
                {
                    try
                    {
                        return CommandHandlers.AllCommandHandlers.ContainsKey(command) && (CommandHandlers.TutorForCurrentSubjectCommandHandlers[userId].ContainsKey(int.Parse(command)));
                    }
                    catch
                    {
                        return command == "Назад";
                    }
                }
                else if (currentState == UserStates.SelectedTutor)
                {
                    return CommandHandlers.AllCommandHandlers.ContainsKey(command) && command == "Назад";
                }
            }
            return false;
        }

        public static async void MessageSendersController(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {

            if (message != null)
            {
                long userId = message.Chat.Id;
                string command = message.Text;

                Console.WriteLine(message.Chat.Id + "\t" + message.Text);

                if (CommandHandlers.AllCommandHandlers == null)
                {
                    FillingCommandHandlers();
                }

                if (!CommandHandlers.UsersState.ContainsKey(userId))
                {
                    CommandHandlers.UsersState[userId] = UserStates.MainMenu;
                }

                if (command == "/start")
                {
                    CommandHandlers.UsersState[userId] = UserStates.MainMenu;
                }
                if (IsCommandIsAllowed(userId, command))
                {
                    if (CommandHandlers.UsersState.TryGetValue(userId, out var states))
                    {
                        if (states == UserStates.MainMenu)
                        {
                            CommandHandlers.StartCommandHandlers[command](botClient, message);
                        }
                        else if (states == UserStates.FindTutor)
                        {
                            CommandHandlers.SubjectCommandHandlers[command](botClient, message);
                        }
                        else if (states == UserStates.Support)
                        {
                            CommandHandlers.SupportCommandHandlers[command](botClient, message);
                        }
                        else if (states == UserStates.SelectedSubject)
                        {
                            if (command == "Назад")
                            {
                                SenderOfSchoolSubjectsListMessage.SendSubjectsListMessage(botClient, message);
                            }
                            else
                            {
                                CommandHandlers.TutorForCurrentSubjectCommandHandlers[userId][int.Parse(command)](botClient, message);
                            }
                        }
                        else if(states == UserStates.SelectedTutor)
                        {
                            if (command == "Назад")
                            {
                                SenderOfSchoolSubjectsListMessage.SendSubjectsListMessage(botClient, message);
                            }
                            else
                            {
                                CommandHandlers.SelectedTutorCommandHandlers[command](botClient, message);
                            }
                        }
                    }
                }
                else
                {
                    await botClient.SendTextMessageAsync(
                        chatId: userId,
                        replyMarkup: new ReplyKeyboardRemove(),
                        text: "Команда не распознана. Для продолжения напишите \"/start\".");
                }
            }
        }
    }
}