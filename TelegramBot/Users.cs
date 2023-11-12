namespace TelegramBot
{
    public static class Users
    {
        public static Dictionary<long, List<int>> MessageToDelete = new Dictionary<long, List<int>>();

        public static Dictionary<long, int> LastMessage = new Dictionary<long, int>();

        public static Dictionary<long, UserStates> UsersState = new Dictionary<long, UserStates>();
    }
}
