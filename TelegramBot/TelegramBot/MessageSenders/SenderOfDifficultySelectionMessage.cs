using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bots.Types;

namespace TelegramBot.MessageSenders
{
    public class SenderOfDifficultySelectionMessage
    {
        public static async void SendDifficultySelectionMessage(ITelegramBotClient botClient, Message message)
        {
            var chatId = message.Chat.Id;

            CommandHandlers.UsersState[chatId] = UserStates.DifficultySelection;
        }
    }
}
