using Microsoft.Extensions.Configuration;
using System;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot
{
    public class BotClient
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

        private static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Task.Run(async () => await MessageHandler.SendMessageHandler(botClient, update, cancellationToken));
        }

        private static async Task ErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {

        }
    }


}
