using Telegram.Bot.Types;

namespace TelegramBot
{
    public static class Users
    {
        public static Dictionary<long, List<int>> MessageToDelete = new Dictionary<long, List<int>>();

        public static Dictionary<long, Update> previousMessage = new Dictionary<long, Update>();
    }
}
