using Telegram.Bot.Types;

namespace TelegramBot
{
    public static class Users
    {
        public static Dictionary<long, List<int>> MessageToDelete = new Dictionary<long, List<int>>();
    }
}
