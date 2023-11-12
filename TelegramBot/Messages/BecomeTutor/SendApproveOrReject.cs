using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Messages.BecomeTutor
{
    public static partial class Messages
    {
        public static async Task SendApproveOrRejectMessage(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string status, string userId)
        {
            if (status == "1")
            {

                string text = update.CallbackQuery.Message.Text;

                text = text.Remove(0, 17);
                text = text.Insert(0, "Завяка одобрена");

                await botClient.EditMessageTextAsync(
                    chatId: update.CallbackQuery.Message.Chat.Id,
                    messageId: update.CallbackQuery.Message.MessageId,
                    text: text);

                InlineKeyboardMarkup inlineKeyboard = new(new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Найти репетитора", callbackData: "find_tutor"),
                        InlineKeyboardButton.WithCallbackData(text: "Стать репетитором", callbackData: "become_tutor")
                    }
                });

                try
                {
                    await botClient.DeleteMessageAsync(
                        chatId: userId,
                        messageId: Users.LastMessage[long.Parse(userId)]);
                }
                catch { }

                if (Users.MessageToDelete.ContainsKey(long.Parse(userId)))
                {
                    List<int> ids = Users.MessageToDelete[long.Parse(userId)];
                    foreach(var id in ids)
                    {
                        try
                        {
                            await botClient.DeleteMessageAsync(
                                chatId: userId,
                                messageId: id);
                        }
                        catch { }
                    }
                }

                await botClient.SendTextMessageAsync(
                        chatId: long.Parse(userId),
                        text: "Ваша заявка одобрена.\n\nВас привествует электронный помощник по поиску репетиторов.\n\nЕсли здесь нет вашего предмета, то напишите сюда: @FindTutor_Support.",
                        replyMarkup: inlineKeyboard,
                        cancellationToken: cancellationToken);
            }
            else
            {
                string text = update.CallbackQuery.Message.Text;

                text = text.Remove(0, 17);
                text = text.Insert(0, "Завяка отклонена");

                await botClient.EditMessageTextAsync(
                    chatId: update.CallbackQuery.Message.Chat.Id,
                    messageId: update.CallbackQuery.Message.MessageId,
                    text: text);

                InlineKeyboardMarkup inlineKeyboard = new(new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Найти репетитора", callbackData: "find_tutor"),
                        InlineKeyboardButton.WithCallbackData(text: "Стать репетитором", callbackData: "become_tutor")
                    }
                });

                try
                {
                    await botClient.DeleteMessageAsync(
                        chatId: userId,
                        messageId: Users.LastMessage[long.Parse(userId)]);
                }
                catch { }

                if (Users.MessageToDelete.ContainsKey(long.Parse(userId)))
                {
                    List<int> ids = Users.MessageToDelete[long.Parse(userId)];
                    foreach (var id in ids)
                    {
                        try
                        {
                            await botClient.DeleteMessageAsync(
                                chatId: userId,
                                messageId: id);
                        }
                        catch { }
                    }
                }

                await botClient.SendTextMessageAsync(
                        chatId: long.Parse(userId),
                        text: "Ваша заявка отклонена. Подробнее можно узнать тут: @FindTutor_Support.\n\nВас привествует электронный помощник по поиску репетиторов.\n\nЕсли здесь нет вашего предмета, то напишите сюда: @FindTutor_Support.",
                        replyMarkup: inlineKeyboard,
                        cancellationToken: cancellationToken);
            }
        }
    }
}