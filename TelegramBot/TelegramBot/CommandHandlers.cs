using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot
{
    public static class CommandHandlers
    {
        public static Dictionary<long, UserStates> UsersState = new Dictionary<long, UserStates>();

        public static Dictionary<string, string> AllCommandHandlers;

        public static Dictionary<string, Action<ITelegramBotClient, Message>> StartCommandHandlers;

        public static Dictionary<string, Action<ITelegramBotClient, Message>> DifficultySelectionCommandHandlers;

        public static Dictionary<string, Action<ITelegramBotClient, Message>> SubjectCommandHandlers;

        public static Dictionary<string, Action<ITelegramBotClient, Message>> SupportCommandHandlers;

        public static Dictionary<long, Dictionary<int, Action<ITelegramBotClient, Message>>> TutorForCurrentSubjectCommandHandlers = new Dictionary<long, Dictionary<int, Action<ITelegramBotClient, Message>>>();

        public static Dictionary<long, string> UserSelectedSubjectHandler = new Dictionary<long, string>();

        public static Dictionary<string, Action<ITelegramBotClient, Message>> SelectedTutorCommandHandlers;
    }
}
