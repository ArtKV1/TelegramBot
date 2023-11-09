namespace TelegramBot
{
    public static class Users
    {
        public static Dictionary<long, List<int>> MessageToDelete = new Dictionary<long, List<int>>();

        public static Dictionary<long, UserStates> UsersState = new Dictionary<long, UserStates>();
    }
}
