using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Messages
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

                await botClient.SendTextMessageAsync(
                    chatId: long.Parse(userId),
                    text: "Ваша заявка одобрена.");

                InlineKeyboardMarkup inlineKeyboard = new(new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Найти репетитора", callbackData: "find_tutor"),
                        InlineKeyboardButton.WithCallbackData(text: "Стать репетитором", callbackData: "become_tutor")
                    }
                });

                await botClient.SendTextMessageAsync(
                        chatId: long.Parse(userId),
                        text: "Вас привествует электронный помощник по поиску репетиторов.\n\nЕсли здесь нет вашего предмета, то напишите сюда: @FindTutor_Support.",
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

                await botClient.SendTextMessageAsync(
                    chatId: long.Parse(userId),
                    text: "Ваша заявка отклонена. Подробнее можно узнать тут: @FindTutor_Support");

                InlineKeyboardMarkup inlineKeyboard = new(new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Найти репетитора", callbackData: "find_tutor"),
                        InlineKeyboardButton.WithCallbackData(text: "Стать репетитором", callbackData: "become_tutor")
                    }
                });

                await botClient.SendTextMessageAsync(
                        chatId: long.Parse(userId),
                        text: "Вас привествует электронный помощник по поиску репетиторов.\n\nДанный бот работает в тестом режиме, если есть какие-то пожелания, то пишите @FindTutor_Support.",
                        replyMarkup: inlineKeyboard,
                        cancellationToken: cancellationToken);
            }
        }
    }
}