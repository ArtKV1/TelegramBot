using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Messages
{
    public static partial class Messages
    {
        public static async Task SendApproveOrRejectMessage(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string status, string userId, string educationalInstitution, string contacts, string description, string fullname, string subjId)
        {
            if (status == "1")
            {
                var connString = "Host=localhost;Port=5432;Username=postgres;Password=123;Database=TelegramBot";

                await using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    if (educationalInstitution == "school")
                    {

                        var cmd = new NpgsqlCommand($"INSERT INTO school_teachers (subjectid, fullname, description, telegram) VALUES ({subjId}, '{fullname}', '{description}', '{contacts}')", conn);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    else if (educationalInstitution == "university")
                    {
                        var cmd = new NpgsqlCommand($"INSERT INTO university_teachers (subjectid, fullname, description, telegram) VALUES ({subjId}, '{fullname}', '{description}', '{contacts}')", conn);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }

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
                        text: "Вас привествует электронный помощник по поиску репетиторов.\n\nДанный бот работает в тестом режиме, если есть какие-то пожелания, то пишите @FindTutor_Support.",
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
