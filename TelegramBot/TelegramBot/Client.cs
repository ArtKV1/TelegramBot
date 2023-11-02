using Microsoft.Extensions.Configuration;
using System;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot
{
    class Client
    {

        public static void CreateClient()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@"E:\Documents\appsettings.json");
            IConfiguration configuration = builder.Build();
            string token = configuration["Token"];

            var client = new TelegramBotClient(token);
            client.StartReceiving(UpdateHandler, ErrorHandler);
            Console.ReadLine();
        }

        private async static Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;
            MessageHandler.MessageSendersController(botClient, message, cancellationToken);
            //MessageHandler.BotInWork(botClient, message, cancellationToken);
        }

        private async static Task ErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            
        }
    }
}
